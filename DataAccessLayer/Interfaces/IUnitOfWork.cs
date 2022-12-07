using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Chat> Chats { get; }
        IRepository<Message> Messages { get; }
        IRepository<User> Users { get; }
        IRepository<Friendship> Friendships { get; }

        Task SaveChangesAsync();
    }
}
