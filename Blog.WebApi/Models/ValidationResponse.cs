using System.Collections.Generic;

namespace Blog.WebApi.Models
{
    public class ValidationResponse
    {
        public string Key { get; set; }
        public List<string> Validations { get; set; }
    }
}