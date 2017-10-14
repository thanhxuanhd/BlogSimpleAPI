using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public string Sex { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
