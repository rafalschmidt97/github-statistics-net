using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GithubStatistics.WebAPI.IntegrationTest.Repositories
{
    public class RepositoriesTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public RepositoriesTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetRepositoriesShouldReturnData()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/repositories");
            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Hello World", body);
        }
    }
}
