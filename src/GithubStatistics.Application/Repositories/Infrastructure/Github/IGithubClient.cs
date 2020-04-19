using System.Threading.Tasks;
using GraphQL;

namespace GithubStatistics.Application.Repositories.Infrastructure.Github
{
    public interface IGithubClient
    {
        public Task<GraphQLResponse<T>> SendQuery<T>(GraphQLRequest request);
    }
}
