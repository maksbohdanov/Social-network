namespace BuisnessLogicLayer.Models.DTOs
{
    public class MessageDto: BaseEntity
    {
        public string Text { get; set; }
        public string AuthorId { get; set; }
        public string ChatId { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
