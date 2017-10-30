using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Core.Model
{
    public class PostTag
    {
        [Key]
        [Column(Order = 1)]
        public Guid PostID { set; get; }

        [Key]
        [Column(Order = 2)]
        [MaxLength(50)]
        public Guid TagID { set; get; }

        public virtual Post Post { set; get; }

        public virtual Tag Tag { set; get; }
    }
}
