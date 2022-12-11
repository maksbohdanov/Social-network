namespace BuisnessLogicLayer.Models.DTOs
{
    public class FriendshipDto: BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string FriendId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsAccepted { get; set; }
    }
}
