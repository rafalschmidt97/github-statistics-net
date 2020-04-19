using System;
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
    public class GithubClient : IGithubClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<GithubClient> _logger;

        public GithubClient(IConfiguration configuration, IHttpClientFactory clientFactory, ILogger<GithubClient> logger)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<GraphQLResponse<T>> SendQuery<T>(GraphQLRequest request)
        {
            GraphQLResponse<T> response;
            try
            {
                var githubClient = PrepareGithubClient();
                response = await githubClient.SendQueryAsync<T>(request);
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

            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _configuration["GITHUB_AUTH_TOKEN"]);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "GithubStatistics");

            return new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), httpClient);
        }
    }
}
