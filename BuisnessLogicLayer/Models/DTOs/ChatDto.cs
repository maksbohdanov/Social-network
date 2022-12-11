namespace BuisnessLogicLayer.Models.DTOs
{
    public class ChatDto: BaseEntity
    {
        public ICollection<string> Messages { get; set; } = new HashSet<string>();
        public ICollection<string> Users { get; set; } = new HashSet<string>();
    }
}
