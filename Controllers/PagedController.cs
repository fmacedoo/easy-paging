namespace dotnet_paging.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class PagedController : ControllerBase
    {
        [HttpGet("api/{provider}/paged/{page}/{pageSize?}")]
        public ActionResult Get()
        {
            var (provider, page, pageSize) = (
                Request.RouteValues["provider"], 
                Request.RouteValues["page"], 
                Request.RouteValues["pageSize"]
            );

            return this.Ok(new { provider, page, pageSize });
        }
    }
}
