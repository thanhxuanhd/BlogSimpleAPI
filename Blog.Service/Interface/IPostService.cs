using Blog.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Service.Interface
{
    public interface IPostService
    {
        List<PostViewModel> Get(string keyword, bool desc = false, int pageIndex = 0, int pageSize = 15);

        Guid Add(PostViewModel post);

        void Update(PostViewModel post);

        PostViewModel GetById(Guid id);

        void Save();

        Task SaveChangesAsync();
    }
}