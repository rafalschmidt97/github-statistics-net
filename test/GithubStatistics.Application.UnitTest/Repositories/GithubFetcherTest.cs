using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GithubStatistics.Application.Repositories.Infrastructure.Github;
using GithubStatistics.Common.Exceptions;
using GraphQL;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GithubStatistics.Application.UnitTest.Repositories
{
    public class GithubFetcherTest
    {
        private readonly Mock<IGithubClient> _githubClient;
        private readonly Mock<ILogger<GithubFetcher>> _logger;

        public GithubFetcherTest()
        {
            _githubClient = new Mock<IGithubClient>();
            _logger = new Mock<ILogger<GithubFetcher>>();
        }

        [Fact]
        public async Task ShouldReturnRepositoriesDetails()
        {
            var repositories = new List<RepositoryDetails>
            {
                new RepositoryDetails { Name = "ABC", StargazersCount = 5, WatchersCount = 10, ForkCount = 20, DiskUsage = 1000 },
                new RepositoryDetails { Name = "a-c-e", StargazersCount = 10, WatchersCount = 20, ForkCount = 40, DiskUsage = 2000 },
            };

            _githubClient
                .Setup(x => x.SendQuery<GithubResponse>(It.IsAny<GraphQLRequest>()))
                .ReturnsAsync(GithubFetcherTestUtilities.MapToResponse(repositories));

            var fetcher = new GithubFetcher(_githubClient.Object, _logger.Object);

            var result = await fetcher.GetRepositoriesDetails("owner");

            result.Should().BeEquivalentTo(repositories);
        }

        [Fact]
        public async Task ShouldReturnAllWithMultiplePages()
        {
            _githubClient
                .SetupSequence(x => x.SendQuery<GithubResponse>(It.IsAny<GraphQLRequest>()))
                .ReturnsAsync(GithubFetcherTestUtilities.GenerateResponse(100, 150))
                .ReturnsAsync(GithubFetcherTestUtilities.GenerateResponse(50, 150));

            var fetcher = new GithubFetcher(_githubClient.Object, _logger.Object);

            var result = await fetcher.GetRepositoriesDetails("owner");

            result.Should().HaveCount(150);
        }

        [Fact]
        public async Task ShouldReturnAllWithModifiedRepositories()
        {
            _githubClient
                .SetupSequence(x => x.SendQuery<GithubResponse>(It.IsAny<GraphQLRequest>()))
                .ReturnsAsync(GithubFetcherTestUtilities.GenerateResponse(100, 150))
                .ReturnsAsync(GithubFetcherTestUtilities.GenerateResponse(50, 140));

            var fetcher = new GithubFetcher(_githubClient.Object, _logger.Object);

            var result = await fetcher.GetRepositoriesDetails("owner");

            result.Should().HaveCount(150); // no tracking for removed repositories while gathering data
        }

        [Fact]
        public async Task ShouldReturnOnePages()
        {
            _githubClient
                .SetupSequence(x => x.SendQuery<GithubResponse>(It.IsAny<GraphQLRequest>()))
                .ReturnsAsync(GithubFetcherTestUtilities.GenerateResponse(100, 150))
                .ReturnsAsync(GithubFetcherTestUtilities.GenerateResponse(50, 150));

            var fetcher = new GithubFetcher(_githubClient.Object, _logger.Object);

            var result = await fetcher.GetRepositoriesDetails("owner", false);

            result.Should().HaveCount(100);
        }

        [Fact]
        public void ShouldThrowNotFound()
        {
            _githubClient
                .Setup(x => x.SendQuery<GithubResponse>(It.IsAny<GraphQLRequest>()))
                .ReturnsAsync(GithubFetcherTestUtilities.GenerateErrorResponse("Could not resolve to a User with the login of 'owner'."));

            var fetcher = new GithubFetcher(_githubClient.Object, _logger.Object);

            Func<Task> result = () => fetcher.GetRepositoriesDetails("owner");

            result.Should().ThrowExactly<NotFoundException>().WithMessage("User 'owner' not found");
        }

        [Fact]
        public void ShouldThrowInternalError()
        {
            _githubClient
                .Setup(x => x.SendQuery<GithubResponse>(It.IsAny<GraphQLRequest>()))
                .ReturnsAsync(GithubFetcherTestUtilities.GenerateErrorResponse("Unexpected query error"));

            var fetcher = new GithubFetcher(_githubClient.Object, _logger.Object);

            Func<Task> result = () => fetcher.GetRepositoriesDetails("owner");

            result.Should().ThrowExactly<InternalException>().WithMessage("Unexpected fetching user repositories error");
        }
    }
}
