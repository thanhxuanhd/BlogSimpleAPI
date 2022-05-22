using BlogSimple.BlazorApp.Models;
using System.Threading.Tasks;

namespace BlogSimple.BlazorApp.Data.Interfaces;

public interface ITokenSevice
{
    public Task<AuthenResponseModel> GetAccessToken(LoginViewModel model);

    public Task Logout();
}