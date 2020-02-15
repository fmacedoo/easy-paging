namespace dotnet_paging.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    public class PagedAttribute : System.Attribute
    {
        public PagedAttribute(string key, System.Type type) => (Key,Type) = (key,type);

        public string Key { get; set; }
        public System.Type Type { get; set; }
    }

    public interface IPagedResults
    {
        object SearchPaged(int page, int? pageSize = null);
    }

    [ApiController]
    public class PagedController : ControllerBase
    {
        readonly static Dictionary<string, Type> _services;

        static PagedController()
        {
            _services = GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly())
                .ToDictionary(o => o.Key, o => o.Type);
        }

        static IEnumerable<PagedAttribute> GetTypesWithHelpAttribute(Assembly assembly) {
            foreach(Type type in assembly.GetTypes()) {
                if (type.GetCustomAttributes(typeof(PagedAttribute), true).Length > 0) {
                    yield return type.GetCustomAttribute<PagedAttribute>();
                }
            }
        }

        IServiceProvider _serviceProvider;

        public PagedController(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        [HttpGet("api/{key}/paged/{page}/{pageSize?}")]
        public ActionResult Get()
        {
            var (key, page, pageSize) = (
                Request.RouteValues["key"].ToString(), 
                Request.RouteValues["page"].ToString(), 
                Request.RouteValues["pageSize"].ToString()
            );

            if (!_services.Keys.Contains(key)) return BadRequest("No provider found!");
            var type = _services[key];

            var service = _serviceProvider.GetService(type) as IPagedResults;
            if (service == null) return BadRequest("No provider found!");

            var pageParam = int.TryParse(page, out _) ? int.Parse(page) : -1;
            if (pageParam == -1) return BadRequest("Page required!");

            var pageSizeParam = int.TryParse(pageSize, out _) ? (int?)int.Parse(pageSize) : null;

            var result = service.SearchPaged(pageParam, pageSizeParam);

            return this.Ok(result);
        }
    }
}
