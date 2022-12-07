using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class FriendshipRepository : Repository<Friendship>
    {
        public FriendshipRepository(SocialNetworkDbContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Friendship>> GetAllAsync()
        {
            return await _context.Friendships
                .Include(x => x.User)
                .Include(x => x.Friend)
                .ToListAsync();
        }
    }
}
