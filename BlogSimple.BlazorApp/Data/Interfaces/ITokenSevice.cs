using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogSimple.BlazorApp.Models;

namespace BlogSimple.BlazorApp.Data.Interfaces
{
    public interface ITokenSevice
    {
        public Task<AuthenResponseModel> GetAccessToken(LoginViewModel model);

        public Task Logout();
    }
}
