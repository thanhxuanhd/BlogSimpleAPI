using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Core.Model
{
    public class PostCategory : IEntityBase
    {
        public PostCategory()
        {
            PostCategories = new List<PostCategory>();
            Posts = new List<Post>();
        }

        [StringLength(50)]
        public string CategoryName { get; set; }
        [StringLength(5000)]
        public string CategoryDescription { get; set; }
        public bool IsPublic { get; set; }
        public Guid? ParentId { get; set; }
        public string Url { get; set; }
        public string MetaData { get; set; }
        public string MetaDescription { set; get; }

        public virtual ICollection<Post> Posts { get; set; }
        [ForeignKey("ParentId")]
        public virtual ICollection<PostCategory> PostCategories { get; set; }
    }
}
