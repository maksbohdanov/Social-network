using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class ChatRepository : Repository<Chat>
    {
        public ChatRepository(SocialNetworkDbContext context) : base(context)
        {
        }
    }
}
