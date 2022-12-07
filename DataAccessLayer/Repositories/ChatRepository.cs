using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ChatRepository : Repository<Chat>
    {
        public ChatRepository(SocialNetworkDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Chat>> GetAllAsync()
        {
            return await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .ToListAsync();
        }
    }
}
