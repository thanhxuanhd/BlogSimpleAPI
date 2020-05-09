using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlogSimple.BlazorApp.Data.Interfaces;
using BlogSimple.BlazorApp.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlogSimple.BlazorApp.Data.Services
{
    public class TokenService : ITokenSevice
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly APIConfiguration _apiOptions;
        private readonly ILogger<TokenService> _logger;

        public TokenService(HttpClient httpClient,
                            ILocalStorageService localStorage,
                            AuthenticationStateProvider authenticationStateProvider,
                            IOptions<APIConfiguration> apiOptions,
                            ILogger<TokenService> logger)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _apiOptions = apiOptions.Value;
            _logger = logger;

            httpClient.BaseAddress = new Uri(_apiOptions.Url);
        }

        public async Task<AuthenResponseModel> GetAccessToken(LoginViewModel model)
        {
            try
            {
                var loginAsJson = JsonSerializer.Serialize(model);

                var response = await _httpClient.PostAsync($"api/{_apiOptions.Version}/Token/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

                var loginResult = JsonSerializer.Deserialize<AuthenResponseModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (!response.IsSuccessStatusCode)
                {
                    loginResult.Successful = false;
                    return loginResult;
                }

                await _localStorage.SetItemAsync("authToken", loginResult.AuthenToken);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult.Email);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AuthenToken);
                loginResult.Successful = true;
                return loginResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new AuthenResponseModel() { Successful = false };
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}