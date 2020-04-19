using System.Collections.Generic;
using GithubStatistics.Application.Repositories.Infrastructure.Github;
using GraphQL;

namespace GithubStatistics.Application.UnitTest.Repositories
{
    public static class GithubFetcherTestUtilities
    {
        public static GraphQLResponse<GithubResponse> MapToResponse(IList<RepositoryDetails> repositories)
        {
            var response = GenerateDataResponseShape(repositories.Count);

            foreach (var repository in repositories)
            {
                response.Data.User.Repositories.Nodes.Add(GenerateRepositoryResponse(repository));
                response.Data.User.Repositories.Edges.Add(GenerateEdgeResponse(repository.Name));
            }

            return response;
        }

        public static GraphQLResponse<GithubResponse> GenerateResponse(int amount, int totalCount)
        {
            var response = GenerateDataResponseShape(totalCount);

            for (var i = 0; i < amount; i++)
            {
                response.Data.User.Repositories.Nodes.Add(GenerateRepositoryResponse($"name-{i}", i, i, i, i));
                response.Data.User.Repositories.Edges.Add(GenerateEdgeResponse($"cursor-{i}"));
            }

            return response;
        }

        public static GraphQLResponse<GithubResponse> GenerateErrorResponse(string errorMessage)
        {
            return new GraphQLResponse<GithubResponse>
            {
                Errors = new[]
                {
                    new GraphQLError { Message = errorMessage },
                },
            };
        }

        private static GraphQLResponse<GithubResponse> GenerateDataResponseShape(int totalCount)
        {
            return new GraphQLResponse<GithubResponse>
            {
                Data = new GithubResponse
                {
                    User = new GithubUserResponse
                    {
                        Repositories = new GithubUserRepositories
                        {
                            Nodes = new List<GithubRepositoryResponse>(),
                            Edges = new List<GithubRepositoryEdgesResponse>(),
                            TotalCount = totalCount,
                        },
                    },
                },
            };
        }

        private static GithubRepositoryResponse GenerateRepositoryResponse(string name, int forkCount, int stargazersCount, int watchersCount, int diskUsage)
        {
            return new GithubRepositoryResponse
            {
                Name = name,
                ForkCount = forkCount,
                Stargazers = new GithubRepositoryCountResponse
                {
                    TotalCount = stargazersCount,
                },
                Watchers = new GithubRepositoryCountResponse
                {
                    TotalCount = watchersCount,
                },
                DiskUsage = diskUsage,
            };
        }

        private static GithubRepositoryResponse GenerateRepositoryResponse(RepositoryDetails repository)
        {
            return GenerateRepositoryResponse(
                repository.Name,
                repository.ForkCount,
                repository.StargazersCount,
                repository.WatchersCount,
                repository.DiskUsage);
        }

        private static GithubRepositoryEdgesResponse GenerateEdgeResponse(string cursorName)
        {
            return new GithubRepositoryEdgesResponse
            {
                Cursor = cursorName,
            };
        }
    }
}
