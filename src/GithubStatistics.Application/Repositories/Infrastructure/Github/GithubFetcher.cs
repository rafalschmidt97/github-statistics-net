using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GithubStatistics.Common.Exceptions;
using GraphQL;
using Microsoft.Extensions.Logging;

namespace GithubStatistics.Application.Repositories.Infrastructure.Github
{
    public class GithubFetcher
    {
        private readonly IGithubClient _githubClient;
        private readonly ILogger<GithubFetcher> _logger;

        public GithubFetcher(IGithubClient githubClient, ILogger<GithubFetcher> logger)
        {
            _githubClient = githubClient;
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

            // there is no other way to fetch all than multiple queries - the api supports up to 100 records
            while (true)
            {
                request.Variables = new { owner, after };
                var response = await _githubClient.SendQuery<GithubResponse>(request);
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
                    // in case of changing the amount of repositories (eg. user has removed one)
                    // it will still finish the loop if locally there is more nodes than remotely
                    if (repositories.Nodes.Count >= response.Data.User.Repositories.TotalCount)
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

        private void CheckErrors(string owner, GraphQLResponse<GithubResponse> response)
        {
            if (response.Errors != null && response.Errors.Any())
            {
                if (response.Errors[0].Message.Contains("Could not resolve to a User", StringComparison.CurrentCulture))
                {
                    throw new NotFoundException($"User '{owner}' not found");
                }

                _logger.LogCritical($"Unexpected fetching user repositories error: {response.Errors[0].Message}");
                throw new InternalException("Unexpected fetching user repositories error");
            }
        }
    }
}
