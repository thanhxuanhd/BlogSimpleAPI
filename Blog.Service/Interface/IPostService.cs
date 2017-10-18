using Blog.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Interface
{
    public interface IPostService
    {
        List<PostViewModel> Get(int pageIndex, int pageSize, string sortColumn);
        Guid Add(PostViewModel post);
        bool Update(PostViewModel post);
        PostViewModel GetById(Guid id);
    }
}
