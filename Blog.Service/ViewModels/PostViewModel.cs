using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Service.ViewModels
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        [Required]
        public Guid PostCategoryId { get; set; }

        public bool IsPublic { get; set; }
        public string Url { get; set; }

        public string MetaData { get; set; }
        public string MetaDescription { set; get; }

        public List<CommentViewModel> Comments { get; set; }
    }
}