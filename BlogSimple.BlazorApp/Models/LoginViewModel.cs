using System.ComponentModel.DataAnnotations;

namespace BlogSimple.BlazorApp.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool IncludeRefreshToken { get; set; } = false;
}