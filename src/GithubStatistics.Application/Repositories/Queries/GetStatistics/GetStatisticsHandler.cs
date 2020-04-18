using System.Threading.Tasks;
using GithubStatistics.Application.Core.Bus;
using GithubStatistics.Application.Repositories.Infrastructure.Github;
using GithubStatistics.Application.Repositories.Infrastructure.Statistics;

namespace GithubStatistics.Application.Repositories.Queries.GetStatistics
{
    public class GetStatisticsHandler : Handler<GetStatisticsQuery, RepositoriesStatistics>
    {
        private readonly GithubFetcher _githubFetcher;

        public GetStatisticsHandler(GithubFetcher githubFetcher)
        {
            _githubFetcher = githubFetcher;
        }

        public override async Task<RepositoriesStatistics> Handle(GetStatisticsQuery request)
        {
            var repositories = await _githubFetcher.GetRepositoriesDetails(request.Owner);
            return RepositoriesProcessor.PrepareStatistics(request.Owner, repositories);
        }
    }
}
