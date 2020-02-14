using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet_paging.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagingController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get(int pageSize, int page)
        {
            return this.Ok(new {
                page,
                pageSize,
            });
        }
    }
}
