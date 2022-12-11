using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class Chat: IEntity
    {
        public Guid Id { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
        public virtual ICollection<UserChat> Users { get; set; } = new HashSet<UserChat>();
    }
}
