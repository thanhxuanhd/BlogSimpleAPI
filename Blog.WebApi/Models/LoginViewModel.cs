using System.ComponentModel.DataAnnotations;

namespace Blog.WebApi.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IncludeRefreshToken { get; set; } = false;
    }
}