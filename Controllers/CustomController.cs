namespace dotnet_paging.Controllers
{
    using System.Threading.Tasks;
    using dotnet_paging.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Paged("custom", typeof(CustomService))]
    public class CustomController : ControllerBase
    {
        CustomService _customService;

        public CustomController(CustomService customService) => _customService = customService;

        [HttpGet]
        public ActionResult Get()
        {
            var result = _customService
                .Search("fil");

            return this.Ok(result);
        }
    }
}
