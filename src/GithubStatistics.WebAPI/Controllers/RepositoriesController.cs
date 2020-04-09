using Microsoft.AspNetCore.Mvc;

namespace GithubStatistics.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepositoriesController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }
    }
}
