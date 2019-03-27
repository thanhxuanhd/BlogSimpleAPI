using System.Collections.Generic;

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