using System.Collections.Generic;
using System.Text.RegularExpressions;
using GithubStatistics.Application.Repositories.Infrastructure.Github;
using GithubStatistics.Application.Repositories.Queries.GetStatistics;

namespace GithubStatistics.Application.Repositories.Infrastructure.Statistics
{
    public static class RepositoriesProcessor
    {
        public static RepositoriesStatistics PrepareStatistics(string owner, IList<RepositoryDetails> repositories)
        {
            var letters = new Dictionary<char, int>();
            float sumStargazers = 0,
                sumWatchers = 0,
                sumForks = 0,
                sumSize = 0;

            foreach (var repository in repositories)
            {
                FillLetters(repository.Name, letters);
                sumStargazers += repository.StargazersCount;
                sumWatchers += repository.WatchersCount;
                sumForks += repository.ForkCount;
                sumSize += repository.DiskUsage;
            }

            return new RepositoriesStatistics(
                owner,
                letters,
                sumStargazers / repositories.Count,
                sumWatchers / repositories.Count,
                sumForks / repositories.Count,
                sumSize / repositories.Count);
        }

        private static void FillLetters(string repositoryName, IDictionary<char, int> letters)
        {
            var simplified = Regex.Replace(repositoryName.ToLowerInvariant(), "[^a-z]", "");
            foreach (var letter in simplified)
            {
                if (letters.ContainsKey(letter))
                {
                    letters[letter] += 1;
                }
                else
                {
                    letters[letter] = 1;
                }
            }
        }
    }
}
