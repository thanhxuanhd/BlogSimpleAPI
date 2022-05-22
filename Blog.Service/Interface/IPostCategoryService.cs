using Blog.Service.ViewModels;
using System;
using System.Collections.Generic;

namespace Blog.Service.Interface;

public interface IPostCategoryService
{
    PagingViewModel<PostCategoryViewModel> Get(int pageIndex, int pageSize, string keyWord = "", string sortColumn = "");

    Guid Add(PostCategoryViewModel postCategory, Guid currentUserId);

    bool Update(PostCategoryViewModel postCategory, Guid currentUserId);

    PostCategoryViewModel GetById(Guid id);

    bool Delete(Guid id, Guid currentUserId);

    void Save();

    void UpdateParent(PostCategoryUpdateParentViewModel model, Guid currentUserId);

    List<PostCategoryViewModel> GetAll();
}