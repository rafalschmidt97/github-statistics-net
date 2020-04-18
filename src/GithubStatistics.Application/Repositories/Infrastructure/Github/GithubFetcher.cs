using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GithubStatistics.Common.Exceptions;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GithubStatistics.Application.Repositories.Infrastructure.Github
{
    public class GithubFetcher
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GithubFetcher> _logger;

        public GithubFetcher(IConfiguration configuration, ILogger<GithubFetcher> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IList<RepositoryDetails>> GetRepositoriesDetails(string owner, bool all = true)
        {
            var request = new GraphQLRequest
            {
                Query = @"
                query GetRepositories($owner: String!, $after: String) {
                  user(login: $owner) {
                    repositories(first: 100, after: $after) {
                      nodes {
                        name
                        forkCount
                        stargazers(first: 0) {
                          totalCount
                        }
                        watchers(first: 0) {
                          totalCount
                        }
                        diskUsage
                      }
                      edges {
                        cursor
                      }
                      totalCount
                    }
                  }
                }
                ",
                OperationName = "GetRepositories",
                Variables = new { owner },
            };

            var repositories = await FetchData(owner, all, request);
            return GithubMapper.ToDetails(repositories.Nodes);
        }

        private async Task<GithubUserRepositories> FetchData(string owner, bool all, GraphQLRequest request)
        {
            GithubUserRepositories repositories = null;
            string after = null;

            // there is no other way to fetch all than multiple queries - the api support up to 100 records
            // TODO: simplify/split methods for fetching as the file is quite long
            while (true)
            {
                request.Variables = new { owner, after };
                var response = await SendQuery(request);
                CheckErrors(owner, response);

                if (repositories == null)
                {
                    repositories = response.Data.User.Repositories;
                }
                else
                {
                    repositories.Nodes.AddRange(response.Data.User.Repositories.Nodes);
                    repositories.Edges.AddRange(response.Data.User.Repositories.Edges);
                }

                if (all)
                {
                    if (repositories.Nodes.Count == response.Data.User.Repositories.TotalCount)
                    {
                        break;
                    }

                    after = repositories.Edges.Last().Cursor;
                }
                else
                {
                    break;
                }
            }

            return repositories;
        }

        private async Task<GraphQLResponse<GithubResponse>> SendQuery(GraphQLRequest request)
        {
            GraphQLResponse<GithubResponse> response;
            try
            {
                response = await PrepareGithubClient().SendQueryAsync<GithubResponse>(request);
            }
            catch (GraphQLHttpException exception)
            {
                _logger.LogCritical($"Github GraphQL API Error: {exception.HttpResponseMessage.ReasonPhrase}");
                throw new InternalException("Unexpected error occurred while resolving the data from GitHub");
            }

            return response;
        }

        private GraphQLHttpClient PrepareGithubClient()
        {
            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_configuration["GITHUB_API_GRAPHQL_URL"]),
            };

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _configuration["GITHUB_AUTH_TOKEN"]);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "GithubStatistics");

            return new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), httpClient);
        }

        private void CheckErrors(string owner, GraphQLResponse<GithubResponse> response)
        {
            if (response.Errors != null && response.Errors.Any())
            {
                if (response.Errors[0].Message.Contains("Could not resolve to a User", StringComparison.CurrentCulture))
                {
                    throw new NotFoundException($"User '{owner}' not found");
                }

                _logger.LogCritical($"Github GraphQL API Error: {response.Errors[0].Message}");
                throw new InternalException("Unexpected error occurred while resolving the data from GitHub");
            }
        }
    }
}
