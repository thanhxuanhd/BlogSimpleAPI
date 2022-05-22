using System;

namespace Blog.Service.ViewModels;

public class PostCategoryUpdateParentViewModel
{
    public Guid Id { get; set; }

    public Guid ParentId { get; set; }
}