using System.Threading.Tasks;
using BlogSimple.BlazorApp.Data.Interfaces;
using BlogSimple.BlazorApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogSimple.BlazorApp.PageModels
{
    public class LoginPage : ComponentBase
    {
        [BindProperty]
        public LoginViewModel LoginModel { get; set; } = new LoginViewModel();

        [BindProperty]
        public string ErrorMessage { set; get; } = string.Empty;

        [BindProperty]
        public bool DisplayError { get; set; } = false;

        [Inject]
        private ITokenSevice _tokenSevice { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        protected ILogger<LoginPage> Logger { get; set; }

        protected async Task HanderValiadSubmit()
        {
            DisplayError = false;
            ErrorMessage = string.Empty;
            var response = await _tokenSevice.GetAccessToken(LoginModel);

            if (response.Successful)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                ErrorMessage = "Username or password incorrect";
                DisplayError = true;
            }
        }
    }
}