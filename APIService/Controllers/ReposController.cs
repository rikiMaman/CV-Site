using GitHubIntegration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPIs.Controllers
{
    [ApiController]
    [Route("api/repos")]
    public class ReposController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public ReposController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        // שליפת כל הריפוזיטוריז של המשתמש
        [HttpGet("portfolio")]
        public async Task<IActionResult> GetPortfolio()
        {
            var repos = await _gitHubService.GetPortfolioAsync();
            return Ok(repos);
        }

        // חיפוש ריפוזיטוריז לפי שם, שפה או שם משתמש
        [HttpGet("search")]
        public async Task<IActionResult> SearchRepositories(
            [FromQuery] string repoName = "",
            [FromQuery] string language = "",
            [FromQuery] string userName = "")
        {
            if (string.IsNullOrWhiteSpace(repoName) && string.IsNullOrWhiteSpace(language) && string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest("At least one search parameter (repoName, language, or userName) is required.");
            }

            try
            {
                var repositories = await _gitHubService.SearchRepositoriesAsync(repoName, language, userName);

                if (repositories == null || repositories.Count == 0)
                {
                    return NotFound("No repositories found matching the criteria.");
                }

                return Ok(repositories);
            }
            catch (Octokit.ApiValidationException ex)
            {
                return BadRequest($"GitHub API error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // שליפת ריפוזיטוריז ב-C# בלבד
        [HttpGet("csharp-repos")]
        public async Task<IActionResult> GetCSharpRepositories()
        {
            var repositories = await _gitHubService.SearchRepositoriesInCSharp();
            return Ok(repositories);
        }
    }
}
