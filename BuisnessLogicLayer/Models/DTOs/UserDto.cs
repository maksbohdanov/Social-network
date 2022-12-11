namespace BuisnessLogicLayer.Models.DTOs
{
    public class UserDto: BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<string> Chats { get; set; } = new HashSet<string>();
    }
}
