using Blog.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Interface
{
    public interface IPostCategoryService
    {
        List<PostCategoryViewModel> Get(int pageIndex, int pageSize, string sortColumn);
        Guid Add(PostCategoryViewModel post);
        bool Update(PostCategoryViewModel post);
        PostCategoryViewModel GetById(Guid id);
    }
}
