using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Core.Model
{
    public class Post : IEntityBase
    {
        [StringLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        public Guid PostCategoryId { get; set; }
        public bool IsPublic { get; set; }
        public string Url { get; set; }

        public string MetaData { get; set; }

        public string MetaDescription { set; get; }

        public virtual IList<Comment> Comments { get; set; }
    }
}