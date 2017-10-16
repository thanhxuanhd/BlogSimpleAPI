using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Service.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
