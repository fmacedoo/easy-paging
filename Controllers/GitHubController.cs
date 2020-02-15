namespace dotnet_paging.Controllers
{
    using System.Threading.Tasks;
    using dotnet_paging.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Paged("github", typeof(GitHubService))]
    public class GitHubController : ControllerBase
    {
        GitHubService _gitHubService;

        public GitHubController(GitHubService gitHubService) => _gitHubService = gitHubService;

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _gitHubService
                .Search(new string[] { "snake" }, new string[] { "C#" }, sort: "stars", page: 1);

            return this.Ok(result);
        }
    }
}
