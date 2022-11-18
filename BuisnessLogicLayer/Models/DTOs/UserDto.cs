namespace BuisnessLogicLayer.Models.DTOs
{
    public class UserDto: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<string> Chats { get; set; }
        //public ICollection<string> Messages { get; set; }
    }
}
