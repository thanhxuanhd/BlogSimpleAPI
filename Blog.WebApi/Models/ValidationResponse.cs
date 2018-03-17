using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WebApi.Models
{
    public class ValidationResponse
    {
        public string Key { get; set; }
        public List<string> Errors { get; set; }
    }
}
