using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class Chat: IEntity
    {
        public Guid Id { get; set; }

        public virtual List<Message> Messages { get; set; }
        public virtual List<UserChat> Users { get; set; }
    }
}
