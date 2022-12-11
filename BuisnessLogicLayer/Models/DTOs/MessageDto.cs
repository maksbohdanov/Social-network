namespace BuisnessLogicLayer.Models.DTOs
{
    public class MessageDto: BaseEntity
    {
        public string Text { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public string ChatId { get; set; } = string.Empty;
        public DateTime TimeCreated { get; set; }
    }
}
