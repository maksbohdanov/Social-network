using DataAccessLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace SocialNetwork.Tests.Helpers
{
    internal class RepositoryHelper
    {
        public static DbContextOptions<SocialNetworkDbContext> GetForumDbOptions()
        {
            var options = new DbContextOptionsBuilder<SocialNetworkDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            
            using (var context = new SocialNetworkDbContext(options))
            {
                SeedData(context);
            }
            

            return options;
        }


        private static void SeedData(SocialNetworkDbContext context)
        {
            //var roles = new List<IdentityRole<Guid>>()
            //{
            //    new IdentityRole<Guid>{Name = "Admin"},
            //    new IdentityRole<Guid>{Name = "User"},
            //};            ;
            //context.Roles.AddRange(roles);

            var users = new List<User>()
            {
                new User(){FirstName = "FirstName1", LastName = "LastName1", City = "City1", BirthDate = DateTime.Today, Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")},
                new User(){FirstName = "FirstName2", LastName = "LastName2", City = "City2", BirthDate = DateTime.Today, Id = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66")},
                new User(){FirstName = "FirstName3", LastName = "LastName3", City = "City3", BirthDate = DateTime.Today, Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7")},
                new User(){FirstName = "FirstName4", LastName = "LastName4", City = "City4", BirthDate = DateTime.Today, Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae8")}
            };
            context.Users.AddRange(users);
            var userChats = new List<UserChat>()
            {
                new UserChat() { User = users[0]},
                new UserChat() { User = users[1]},
                new UserChat() { User = users[0]},
                new UserChat() { User = users[2]},
            };

            var chats = new List<Chat>()
            {
                new Chat(){ Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728951e"), Users = userChats.Take(2).ToList()},
                new Chat(){ Id = new Guid("30dd879c-ee2f-11db-8314-0800200c9a65"), Users = userChats.TakeLast(2).ToList()}
            };
            context.Chats.AddRange(chats);

            

            var messages = new List<Message>()
            {
                new Message(){ Text = "Hi!", Author = users[0], Chat = chats[0], Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")},
                new Message(){ Text = "Hello!", Author = users[1], Chat = chats[0], Id = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66")},
                new Message(){ Text = "How are you?", Author = users[0], Chat = chats[0], Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7")},
                new Message(){ Text = "Text1", Author = users[0], Chat = chats[1], Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae1")},
                new Message(){ Text = "Text2", Author = users[2], Chat = chats[1], Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae2")},
                new Message(){ Text = "Text3", Author = users[0], Chat = chats[1], Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae3")},
            };
            context.Messages.AddRange(messages);


            context.SaveChanges();
        }           
    }
}
