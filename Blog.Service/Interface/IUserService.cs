using Blog.Service.ViewModels;
using System;
using System.Threading.Tasks;

namespace Blog.Service.Interface;

public interface IUserService
{
    Guid Add(UserViewModel user);

    bool Update(UserUpdateViewModel user);

    bool Delete(Guid userId);

    PagingViewModel<UserViewModel> GetList(int page, int pageSize, string keyWord = "", string sort = "", bool desc = false);

    UserWidthRoleViewModel GetById(Guid Id);

    void Save();

    Task SaveChangesAsync();
}