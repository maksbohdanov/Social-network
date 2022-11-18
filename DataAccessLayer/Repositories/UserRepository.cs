using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(SocialNetworkDbContext context) : base(context)
        {
        }
    }
}
