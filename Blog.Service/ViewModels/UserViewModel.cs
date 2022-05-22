using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Service.ViewModels;

public class UserViewModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string RePassword { get; set; }

    public string FullName { get; set; }

    public DateTime BirthDay { get; set; }

    public Guid Id { get; set; }

    public IList<Guid> RoleIds { get; set; } = new List<Guid>();

    public string PhoneNumber { get; set; }
    public string Sex { get; set; }
}

public class UserUpdateViewModel
{
    [Required]
    [MaxLength(256)]
    public string FullName { get; set; }

    public DateTime BirthDay { get; set; }

    [Required]
    public Guid Id { get; set; }

    public string PhoneNumber { get; set; }

    public string Sex { get; set; }
}