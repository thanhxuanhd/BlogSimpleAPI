using Blog.Core.Interface;
using Blog.Core.Model;
using Blog.Infrastructure;

namespace Blog.Core.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(BlogDbContext context) : base(context)
        {
        }
    }
}