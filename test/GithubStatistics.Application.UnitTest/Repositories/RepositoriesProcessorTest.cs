using System.Collections.Generic;
using GithubStatistics.Application.Repositories.Infrastructure.Github;
using GithubStatistics.Application.Repositories.Infrastructure.Statistics;
using Xunit;

namespace GithubStatistics.Application.UnitTest.Repositories
{
    public class RepositoriesProcessorTest
    {
        [Fact]
        public void ShouldReturnStatistics()
        {
            var result = RepositoriesProcessor
                .PrepareStatistics("owner", GetSampleRepositoryDetails());

            Assert.Equal("owner", result.Owner);
            Assert.Equal(3, result.Letters['v']);
            Assert.Equal(1, result.Letters['a']);
            Assert.False(result.Letters.ContainsKey('z'));
            Assert.False(result.Letters.ContainsKey('A')); // case insensitive
            Assert.False(result.Letters.ContainsKey('-')); // only letters
            Assert.Equal(10, result.AvgStargazers);
            Assert.Equal(10, result.AvgWatchers);
            Assert.Equal(10, result.AvgForks);
            Assert.Equal(1000, result.AvgSize);
        }

        private static IList<RepositoryDetails> GetSampleRepositoryDetails()
        {
            return new List<RepositoryDetails>
            {
                new RepositoryDetails("skelvy-api", 5, 5, 5, 1000),
                new RepositoryDetails("skelvy-client", 10, 10, 10, 1000),
                new RepositoryDetails("skelvy-website", 15, 15, 15, 1000),
            };
        }
    }
}
