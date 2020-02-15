namespace dotnet_paging.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using dotnet_paging.Controllers;
    using GitHubSharp;
    using GitHubSharp.Models;
    using Microsoft.Extensions.Configuration;

    public class GitHubService : IPagedResults
    {
        readonly (string, string) _credentials;

        public GitHubService(IConfiguration configuration) => _credentials = (
            configuration["github_username"],
            configuration["github_password"]
        );

        public async Task<RepositorySearchModel> Search(string[] keywords, string[] languages, string sort, int page)
        {
            var (username, password) = _credentials;
            var client = GitHubSharp.Client.Basic(username, password);
            var request = client.Repositories.SearchRepositories(keywords, languages, sort, page);

            var response = await client.ExecuteAsync(request);
            var result = response.Data;

            return result;
        }

        public object SearchPaged(int page, int? pageSize = null)
        {
            var (username, password) = _credentials;
            var client = GitHubSharp.Client.Basic(username, password);
            var request = client.Repositories.SearchRepositories(new string[] { "snake" }, new string[] { "c#" }, "stars", page);

            var response = client.ExecuteAsync(request).Result;
            var result = response.Data;

            return result;
        }
    }
}