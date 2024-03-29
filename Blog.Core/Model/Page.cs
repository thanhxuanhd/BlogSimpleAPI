﻿using System.ComponentModel.DataAnnotations;

namespace Blog.Core.Model;

public class Page : IEntityBase
{
    [Required]
    [MaxLength(256)]
    public string Name { set; get; }

    [MaxLength(256)]
    [Required]
    public string Alias { set; get; }

    public string Content { set; get; }
}