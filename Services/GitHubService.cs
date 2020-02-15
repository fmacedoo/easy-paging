namespace dotnet_paging.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GitHubSharp;
    using GitHubSharp.Models;
    using Microsoft.Extensions.Configuration;

    public class GitHubService
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
    }
}