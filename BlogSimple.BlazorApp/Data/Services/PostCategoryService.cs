using Blazored.LocalStorage;
using BlogSimple.BlazorApp.Data.Interfaces;
using BlogSimple.BlazorApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlogSimple.BlazorApp.Data.Services;

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

        Uri clientUrl = new Uri(_apiOption.Url);
        _client.BaseAddress = clientUrl;
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

    public async Task<PostCategoryViewModel> Get(Guid? id)
    {
        PostCategoryViewModel postCategory = new PostCategoryViewModel();
        try
        {
            string url = string.Format(POST_CATEGORY_URL, _apiOption.Version);
            url = $"{url}/{id}";

            await PrepareHeader();

            postCategory = await _client.GetJsonAsync<PostCategoryViewModel>(url);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex.Message);
        }

        return postCategory;
    }

    public async Task<bool> Edit(PostCategoryViewModel model)
    {
        bool result = false;
        try
        {
            string url = string.Format(POST_CATEGORY_URL, _apiOption.Version);

            await PrepareHeader();

            result = await _client.PutJsonAsync<bool>(url, model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex.Message);
        }

        return result;
    }

    public async Task<Guid> Add(PostCategoryViewModel model)
    {
        Guid id = Guid.Empty;
        try
        {
            string url = string.Format(POST_CATEGORY_URL, _apiOption.Version);

            await PrepareHeader();

            id = await _client.PostJsonAsync<Guid>(url, model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex.Message);
        }

        return id;
    }

    public async Task<List<PostCategoryViewModel>> Get()
    {
        try
        {
            string url = string.Format(POST_CATEGORY_URL, _apiOption.Version);
            url = $"{url}/GetAll";

            await PrepareHeader();

            var response = await _client.GetJsonAsync<List<PostCategoryViewModel>>(url);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return new List<PostCategoryViewModel>();
        }
    }

    public async Task PrepareHeader()
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}