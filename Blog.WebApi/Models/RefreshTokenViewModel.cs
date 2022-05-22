using System.ComponentModel.DataAnnotations;

namespace Blog.WebApi.Models;

public class RefreshTokenViewModel
{
    [Required]
    public string Token { get; set; }

    [Required]
    public string RefreshToken { get; set; }
}