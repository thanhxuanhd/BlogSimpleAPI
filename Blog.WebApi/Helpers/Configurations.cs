using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WebApi.Helpers
{
    public class Configurations
    {
        public List<Domain> Domains { get; set; }
    }

    public class Domain
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
