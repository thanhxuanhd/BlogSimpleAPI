using System;
using System.Collections.Generic;

namespace Blog.Service.ViewModels
{
    public class UserWidthRoleViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDay { get; set; }
    }
}