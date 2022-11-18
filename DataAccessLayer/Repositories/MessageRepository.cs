using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class MessageRepository : Repository<Message>
    {
        public MessageRepository(SocialNetworkDbContext context) : base(context)
        {
        }
    }
}
