namespace dotnet_paging.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GitHubSharp;
    using GitHubSharp.Models;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Linq;
    using dotnet_paging.Controllers;

    public class CustomService : IPagedResults
    {
        static readonly List<NomeTuga> _nomes;

        static CustomService()
        {
            var content = System.IO.File.ReadAllText("assets/nomes.json");
            var nomes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(content);
            _nomes = nomes.Select(o => new NomeTuga { Nome = o }).ToList();
        }

        public NomesSearchResult Search(string criteria)
        {
            var result = _nomes
                .Where(o => o.Nome.Contains(criteria, StringComparison.InvariantCultureIgnoreCase));

            return new NomesSearchResult
            {
                Items = result,
                TotalResults = result.Count(),
            };
        }

        public object SearchPaged(int page, int? pageSize = 6)
        {
            var result = _nomes
                .Where(o => o.Nome.Contains("fil", StringComparison.InvariantCultureIgnoreCase));

            return new NomesSearchResult
            {
                Items = result
                    .Skip((page - 1) * pageSize.Value)
                    .Take(pageSize.Value),
                TotalResults = result.Count(),
            };
        }
    }

    public class NomesSearchResult
    {
        public int TotalResults { get; set; }
        public IEnumerable<NomeTuga> Items { get; set; }
    }

    public class NomeTuga
    {
        public string Nome { get; set; }
    }
}