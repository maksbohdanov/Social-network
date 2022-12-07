using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class Friendship: IEntity
    {
        public Guid Id { get; set; }
        public virtual User User { get; set; }
        public Guid? UserId { get; set; }

        public virtual User Friend { get; set; }
        public Guid? FriendId { get; set; }

        public bool IsAccepted { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}