using Blog.Core.Interface;
using Blog.Core.Model;
using Blog.Infrastructure;

namespace Blog.Core.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(BlogDbContext context) : base(context)
    {
    }
}