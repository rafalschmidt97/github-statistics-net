using System.Collections.Generic;

namespace GithubStatistics.Application.Repositories.Infrastructure.Github
{
    public class GithubResponse
    {
        public GithubUserResponse User { get; set; }
    }

    public class GithubUserResponse
    {
        public GithubUserRepositories Repositories { get; set; }
    }

    public class GithubUserRepositories
    {
        public List<GithubRepositoryResponse> Nodes { get; set; }
        public List<GithubRepositoryEdgesResponse> Edges { get; set; }
        public int TotalCount { get; set; }
    }

    public class GithubRepositoryResponse
    {
        public string Name { get; set; }
        public int ForkCount { get; set; }
        public GithubRepositoryCountResponse Stargazers { get; set; }
        public GithubRepositoryCountResponse Watchers { get; set; }
        public int DiskUsage { get; set; }
    }

    public class GithubRepositoryEdgesResponse
    {
        public string Cursor { get; set; }
    }

    public class GithubRepositoryCountResponse
    {
        public int TotalCount { get; set; }
    }
}
