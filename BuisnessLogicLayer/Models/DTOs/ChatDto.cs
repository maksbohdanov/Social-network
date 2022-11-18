namespace BuisnessLogicLayer.Models.DTOs
{
    public class ChatDto: BaseEntity
    {
        public ICollection<string> Messages { get; set; }
        public ICollection<string> Users { get; set; }
    }
}
