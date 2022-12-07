namespace BuisnessLogicLayer.Models.DTOs
{
    public class FriendshipDto: BaseEntity
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAccepted { get; set; }
    }
}
