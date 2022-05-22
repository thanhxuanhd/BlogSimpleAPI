using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.BlazorApp.Models;

public class PostCategoryViewModel
{
    public PostCategoryViewModel()
    {
        this.PostCategories = new List<PostCategoryViewModel>();
        this.Posts = new List<PostViewModel>();
    }

    public Guid Id { get; set; }

    [Required(ErrorMessage = "POST_CATEGORY_NAME_REQUIRED")]
    [StringLength(50)]
    public string CategoryName { get; set; }

    [StringLength(5000)]
    public string CategoryDescription { get; set; }

    public bool IsPublic { get; set; }

    public Guid? ParentPostCategory { get; set; }

    public string Url { get; set; }

    public string MetaData { get; set; }

    public string MetaDescription { set; get; }

    public List<PostViewModel> Posts { get; set; }

    public List<PostCategoryViewModel> PostCategories { get; set; }
}