namespace DataAccessLayer.Entities
{
    public class UserChat
    {
        public virtual User User { get; set; }
        public Guid UserId { get; set; }
        
        public virtual Chat Chat { get; set; }
        public Guid ChatId { get; set; }
    }
}