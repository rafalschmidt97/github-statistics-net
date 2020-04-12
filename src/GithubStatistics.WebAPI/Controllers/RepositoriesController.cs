using System.Threading.Tasks;
using GithubStatistics.Application.Repositories.Queries.GetStatistics;
using GithubStatistics.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GithubStatistics.WebAPI.Controllers
{
    public class RepositoriesController : ApiControllerBase
    {
        /// <summary>
        /// Get repositories statistics for users.
        /// </summary>
        /// <param name="owner">GitHub username.</param>
        /// <returns>Repositories statistics.</returns>
        /// <response code="200">Returns successfully fetched statistics.</response>
        /// <response code="404">Returns an error if the user not exists.</response>
        [HttpGet("{owner}")]
        [ProducesResponseType(typeof(RepositoriesStatistics), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        public async Task<RepositoriesStatistics> GetStatistics(string owner)
        {
            return await Mediator.Send(new GetStatisticsQuery(owner));
        }
    }
}
