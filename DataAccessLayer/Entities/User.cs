using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Entities
{
    public class User: IdentityUser<Guid>, IEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public DateTime BirthDate{ get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<UserChat> Chats { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
