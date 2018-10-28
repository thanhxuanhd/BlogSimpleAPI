using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Service.ViewModels
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }
    }
}