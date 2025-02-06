using GitHubIntegration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPIs.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public UserController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        // שליפת מספר העוקבים של משתמש מסוים
        [HttpGet("{userName}/followers")]
        public async Task<IActionResult> GetUserFollowers(string userName)
        {
            int followers = await _gitHubService.GetUserFollowersAsync(userName);
            return Ok(new { UserName = userName, Followers = followers });
        }

        // שליפת מספר הריפוזיטוריז הציבוריים של משתמש מסוים
        [HttpGet("{userName}/public-repos")]
        public async Task<IActionResult> GetUserPublicRepositories(string userName)
        {
            int publicRepos = await _gitHubService.GetUserPublicRepositories(userName);
            return Ok(new { UserName = userName, PublicRepositories = publicRepos });
        }
    }
}
