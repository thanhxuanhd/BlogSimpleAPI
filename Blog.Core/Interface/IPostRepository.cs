using Blog.Core.Model;
using Blog.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Interface
{
    public interface IPostRepository : IRepository<Post>
    {
    }
}
