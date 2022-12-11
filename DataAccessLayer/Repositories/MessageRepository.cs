using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class MessageRepository : Repository<Message>
    {
        public MessageRepository(SocialNetworkDbContext context) : base(context)
        {
        }

        public override async Task<Message?> GetByIdAsync(string id)
        {
            return (await GetAllAsync())
                .First(x => x.Id.ToString() == id);
        }

        public override async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _context.Messages
                .Include(x => x.Author)
                .Include(x => x.Chat)
                .ToListAsync();
        }
    }
}
