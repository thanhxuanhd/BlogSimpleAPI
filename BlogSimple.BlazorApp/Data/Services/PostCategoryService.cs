using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlogSimple.BlazorApp.Data.Interfaces;
using BlogSimple.BlazorApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlogSimple.BlazorApp.Data.Services
{
    public class PostCategoryService : IPostCategoryService
    {
        private readonly HttpClient _client;

        private readonly APIConfiguration _apiOption;

        private readonly ILogger<PostCategoryService> _logger;

        private readonly ILocalStorageService _localStorage;

        private const string POST_CATEGORY_URL = "/api/{0}/PostCategory";

        public PostCategoryService(HttpClient client,
                                   IOptions<APIConfiguration> apiOption,
                                   ILogger<PostCategoryService> logger,
                                   ILocalStorageService localStorage)
        {
            _client = client;
            _apiOption = apiOption.Value;
            _logger = logger;
            _localStorage = localStorage;
        }

        public async Task<PagingViewModel<PostCategoryViewModel>> Get(string keyWord = "", string sortColunm = "", int pageIndex = 0, int pageSize = 15)
        {
            try
            {
                string url = string.Format(POST_CATEGORY_URL, _apiOption.Version);
                url = $"{url}?keyWord={keyWord}&sortColunm={sortColunm}&pageIndex={pageIndex}&pageSize={pageSize}";

                await PrepareHeader();

                var response = await _client.GetJsonAsync<PagingViewModel<PostCategoryViewModel>>(url);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new PagingViewModel<PostCategoryViewModel>();
            }
        }

        public async Task PrepareHeader()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrWhiteSpace(savedToken))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
            }
            else
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", string.Empty);
            }
        }
    }
}