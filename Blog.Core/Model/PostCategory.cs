using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Core.Model
{
    public class PostCategory : IEntityBase
    {
        [StringLength(50)]
        public string CategoryName { get; set; }
        [StringLength(5000)]
        public string CagegoryDescription { get; set; }
        public bool IsPublic { get; set; }
        public Guid? ParentPostCagegory { get; set; }
        public string Url { get; set; }
        public string MetaData { get; set; }
        public string MetaDescription { set; get; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<PostCategory> PostCagegorys { get; set; }
    }
}
