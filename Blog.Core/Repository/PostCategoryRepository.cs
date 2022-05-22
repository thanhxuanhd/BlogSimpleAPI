using Blog.Core.Interface;
using Blog.Core.Model;
using Blog.Infrastructure;

namespace Blog.Core.Repository;

public class PostCategoryRepository : Repository<PostCategory>, IPostCagegoryRepository
{
    public PostCategoryRepository(BlogDbContext context) : base(context)
    {
    }
}