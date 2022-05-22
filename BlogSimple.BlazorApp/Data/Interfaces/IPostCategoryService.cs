using BlogSimple.BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSimple.BlazorApp.Data.Interfaces;

public interface IPostCategoryService
{
    public Task<PagingViewModel<PostCategoryViewModel>> Get(string keyWord = "", string sortColunm = "", int pageIndex = 0, int pageSize = 15);

    public Task<List<PostCategoryViewModel>> Get();

    public Task<PostCategoryViewModel> Get(Guid? id);

    public Task<bool> Edit(PostCategoryViewModel model);

    public Task<Guid> Add(PostCategoryViewModel model);
}