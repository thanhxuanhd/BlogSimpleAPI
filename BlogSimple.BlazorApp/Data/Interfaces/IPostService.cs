using BlogSimple.BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSimple.BlazorApp.Data.Interfaces;

public interface IPostService
{
    public Task<PagingViewModel<PostViewModel>> Get(string keyWord = "", string sortColunm = "", int pageIndex = 0, int pageSize = 15);

    public Task<List<PostViewModel>> Get();

    public Task<PostViewModel> Get(Guid? id);

    public Task<bool> Edit(PostViewModel model);

    public Task<Guid> Add(PostViewModel model);
}