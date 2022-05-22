using System.ComponentModel.DataAnnotations;

namespace Blog.Core.Model;

public class Tag : IEntityBase
{
    [MaxLength(50)]
    [Required]
    public string Name { set; get; }

    [MaxLength(50)]
    [Required]
    public string Type { set; get; }
}