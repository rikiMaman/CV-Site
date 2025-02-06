using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GitHubIntegration.DataEntities;

namespace GitHubIntegration
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubIntegrationOptions _options;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "GitHubPortfolio";
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5); // זמן תקפות הנתונים ב-Cache

        public GitHubService(IOptions<GitHubIntegrationOptions> options, IMemoryCache cache)
        {
            _client = new GitHubClient(new ProductHeaderValue("my-github-app"))
            {
                Credentials = new Credentials(options.Value.Token) // הזדהות עם GitHub Token
            };
            _options = options.Value;
            _cache = cache;
        }

        public async Task<List<RepoDto>> GetPortfolioAsync()
        {
            string cacheKey = $"{CacheKey}_{_options.UserName}";

            if (!_cache.TryGetValue(cacheKey, out List<RepoDto> repositories))
            {
                var repos = await _client.Repository.GetAllForUser(_options.UserName);
                repositories = new List<RepoDto>();
                foreach (var repo in repos)
                {
                    try
                    {
                        if (repo.PushedAt == null)
                        {
                            continue;
                        }
                        var pullRequests = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);
                        var languagesList = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);

                        var languages = languagesList != null && languagesList.Count > 0
                            ? string.Join(", ", languagesList.Select(lang => lang.Name))
                            : "Unknown";

                        repositories.Add(new RepoDto
                        {
                            Name = repo.Name,
                            Language = languages,
                            LastCommitDate = repo.PushedAt?.DateTime,
                            Stars = repo.StargazersCount,
                            PullRequests = pullRequests.Count,
                            HtmlUrl = repo.HtmlUrl
                        });
                    }
                    catch (Octokit.ApiException ex)
                    {
                        Console.WriteLine($"Error fetching data for repo {repo.Name}: {ex.Message}");
                        continue;
                    }
                }


                _cache.Set(cacheKey, repositories, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheDuration
                });
            }

            return repositories;
        }




        public async Task<int> GetUserFollowersAsync(string userName)
        {
            var user = await _client.User.Get(userName);
            return user.Followers;
        }

        public async Task<int> GetUserPublicRepositories(string userName)
        {
            var user = await _client.User.Get(userName);
            return user.PublicRepos;
        }

        public async Task<List<Repository>> SearchRepositoriesInCSharp()
        {
            var request = new SearchRepositoriesRequest("repo-name")
            {
                Language = Language.CSharp
            };
            var result = await _client.Search.SearchRepo(request);
            return result.Items.ToList();
        }



        public async Task<List<Repository>> SearchRepositoriesAsync(string repoName = "", string language = "", string userName = "")
        {
            var searchTerms = new List<string>();

            if (!string.IsNullOrWhiteSpace(repoName))
                searchTerms.Add(repoName);

            if (!string.IsNullOrWhiteSpace(language))
                searchTerms.Add($"language:{language}");

            if (!string.IsNullOrWhiteSpace(userName))
                searchTerms.Add($"user:{userName}");

            // אם לא הוזן שם ריפו או שם משתמש, נוסיף כוכבים כדי שהחיפוש יהיה תקף
            if (searchTerms.Count == 0)
            {
                throw new ArgumentException("At least one search parameter is required.");
            }

            // אם מחפשים רק לפי שפה, נוסיף stars>0 כדי ש-GitHub API יאשר את החיפוש
            if (searchTerms.Count == 1 && !string.IsNullOrWhiteSpace(language))
            {
                searchTerms.Add("stars:>0");
            }

            var request = new SearchRepositoriesRequest(string.Join(" ", searchTerms)); // תיקון

            try
            {
                var result = await _client.Search.SearchRepo(request);
                return result.Items.ToList(); // המרת IReadOnlyList ל-List
            }
            catch (ApiValidationException ex)
            {
                throw new Exception($"GitHub API error: {ex.Message}");
            }
        }


    }
}
