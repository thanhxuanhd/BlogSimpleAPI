using Blog.Core.Model;
using Blog.Infrastructure;

namespace Blog.Core.Interface;

public interface IPostRepository : IRepository<Post>
{
}