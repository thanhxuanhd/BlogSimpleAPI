using System;
using System.Threading.Tasks;
using Blog.Service.ViewModels;

namespace Blog.Service.Interface
{
    public interface IRoleService
    {
        RoleViewModel GetById(Guid id);

        PagingViewModel<RoleViewModel> GetList(int page, int pageSize, string keyWord = "", string sort = "", bool desc = false);

        Guid Add(RoleViewModel role);

        bool Update(RoleViewModel role);

        bool Delete(Guid Id);

        void Save();

        Task SaveChangesAsync();
    }
}