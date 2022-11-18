using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class Message: IEntity
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeCreated { get; set; }

        public virtual User Author{ get; set; }
        public Guid AuthorId { get; set; }

        public virtual Chat Chat { get; set; }
        public Guid ChatId { get; set; }
    }
}