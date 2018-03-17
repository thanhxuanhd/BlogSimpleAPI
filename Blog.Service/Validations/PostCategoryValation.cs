using Blog.Service.ViewModels;
using FluentValidation;
using System;

namespace Blog.Service.Validations
{
    public class PostCategoryUpdateParentValations : AbstractValidator<PostCategoryUpdateParentViewModel>
    {
        public PostCategoryUpdateParentValations()
        {
            RuleFor(postCategoryUpdateParent => postCategoryUpdateParent.Id).NotEqual(Guid.Empty).WithMessage("POST_CATEGORY_ID_NULL");
            RuleFor(postCategoryUpdateParent => postCategoryUpdateParent.ParentId).NotEqual(Guid.Empty).WithMessage("POST_CATEGORY_PARENT_NULL");
            RuleFor(postCategoryUpdateParent => postCategoryUpdateParent.ParentId).NotEqual(postCategoryUpdateParent => postCategoryUpdateParent.Id).WithMessage("POST_CATEGORY_DUPLICATE");
        }
    }
}