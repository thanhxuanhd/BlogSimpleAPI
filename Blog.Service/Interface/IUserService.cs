using Blog.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Interface
{
    public interface IUserService
    {
        Guid Add(UserViewModel user);
        bool Update(UserViewModel user);
        bool Delete(Guid userId);
        List<UserViewModel> GetList(int page, int pageSize, string keyWord = "", string sort = "", bool desc = false);
    }
}
