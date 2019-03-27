using System;

namespace Blog.Core.Extensions
{
    public class BlogException : Exception
    {
        public BlogException(string message) : base(message)
        {
        }

        public BlogException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}