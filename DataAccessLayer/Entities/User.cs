﻿using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Entities
{
    public class User: IdentityUser<Guid>, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public DateTime BirthDate{ get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<UserChat> Chats { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}