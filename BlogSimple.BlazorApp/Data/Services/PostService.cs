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

public class PostService : IPostService
{
    private readonly HttpClient _client;

    private readonly APIConfiguration _apiOption;

    private readonly ILogger<PostService> _logger;

    private readonly ILocalStorageService _localStorage;

    private const string POST_URL = "/api/{0}/Post";

    public PostService(HttpClient client,
                               IOptions<APIConfiguration> apiOption,
                               ILogger<PostService> logger,
                               ILocalStorageService localStorage)
    {
        _client = client;
        _apiOption = apiOption.Value;
        _logger = logger;
        _localStorage = localStorage;

        Uri clientUrl = new Uri(_apiOption.Url);
        _client.BaseAddress = clientUrl;
    }

    public async Task<PagingViewModel<PostViewModel>> Get(string keyWord = "", string sortColunm = "", int pageIndex = 0, int pageSize = 15)
    {
        try
        {
            string url = string.Format(POST_URL, _apiOption.Version);
            url = $"{url}?keyWord={keyWord}&sortColunm={sortColunm}&pageIndex={pageIndex}&pageSize={pageSize}";

            await PrepareHeader();

            var response = await _client.GetJsonAsync<PagingViewModel<PostViewModel>>(url);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return new PagingViewModel<PostViewModel>();
        }
    }

    public async Task<PostViewModel> Get(Guid? id)
    {
        PostViewModel post = new PostViewModel();
        try
        {
            string url = string.Format(POST_URL, _apiOption.Version);
            url = $"{url}/{id}";

            await PrepareHeader();

            post = await _client.GetJsonAsync<PostViewModel>(url);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex.Message);
        }

        return post;
    }

    public async Task<bool> Edit(PostViewModel model)
    {
        bool result = false;
        try
        {
            string url = string.Format(POST_URL, _apiOption.Version);

            await PrepareHeader();

            result = await _client.PutJsonAsync<bool>(url, model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex.Message);
        }

        return result;
    }

    public async Task<Guid> Add(PostViewModel model)
    {
        Guid id = Guid.Empty;
        try
        {
            string url = string.Format(POST_URL, _apiOption.Version);

            await PrepareHeader();

            id = await _client.PostJsonAsync<Guid>(url, model);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex.Message);
        }

        return id;
    }

    public async Task<List<PostViewModel>> Get()
    {
        try
        {
            string url = string.Format(POST_URL, _apiOption.Version);
            url = $"{url}/GetAll";

            await PrepareHeader();

            var response = await _client.GetJsonAsync<List<PostViewModel>>(url);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return new List<PostViewModel>();
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