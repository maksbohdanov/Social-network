using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly SocialNetworkDbContext _context;
        private readonly IRepository<Chat> _chatRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Friendship> _friendshipRepository;

        public UnitOfWork(SocialNetworkDbContext context, IRepository<Chat> chatRepository,
                        IRepository<Message> messageRepository, IRepository<User> userRepository,
                        IRepository<Friendship> friendshipRepository)
        {
            _context = context;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _friendshipRepository = friendshipRepository;
        }

        public IRepository<Chat> Chats => _chatRepository;

        public IRepository<Message> Messages => _messageRepository;

        public IRepository<User> Users => _userRepository;

        public IRepository<Friendship> Friendships => _friendshipRepository;

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
