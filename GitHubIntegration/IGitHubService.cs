using GitHubIntegration.DataEntities;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIntegration
{
    public interface IGitHubService
    {
        public Task<int> GetUserFollowersAsync(string userName);
        public Task<List<Repository>> SearchRepositoriesInCSharp();
        Task<int> GetUserPublicRepositories(string userName);
        Task<List<RepoDto>> GetPortfolioAsync();
        Task<List<Repository>> SearchRepositoriesAsync(string repoName, string language, string userName);

    }
}
