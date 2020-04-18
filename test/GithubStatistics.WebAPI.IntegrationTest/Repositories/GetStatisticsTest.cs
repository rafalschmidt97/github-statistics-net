using System.Net;
using System.Threading.Tasks;
using GithubStatistics.Application.Repositories.Queries.GetStatistics;
using GithubStatistics.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace GithubStatistics.WebAPI.IntegrationTest.Repositories
{
    public class GetStatisticsTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GetStatisticsTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldReturnData()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/repositories/rafalschmidt97");
            var bodyString = await response.Content.ReadAsStringAsync();
            var body = JsonConvert.DeserializeObject<RepositoriesStatistics>(bodyString);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("rafalschmidt97", body.Owner);
            Assert.InRange(body.AvgStargazers, 0, 10);
        }

        [Fact]
        public async Task ShouldThrowNotFound()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/repositories/fakerafalschmidt");
            var bodyString = await response.Content.ReadAsStringAsync();
            var body = JsonConvert.DeserializeObject<ExceptionResponse>(bodyString);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("User 'fakerafalschmidt' not found", body.Message);
        }
    }
}
