using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Core.Model;

public class PostTag
{
    [Key]
    [Column(Order = 1)]
    public Guid PostID { set; get; }

    [Key]
    [Column(Order = 2)]
    [MaxLength(50)]
    public Guid TagID { set; get; }

    public Post Post { set; get; }

    public Tag Tag { set; get; }
}