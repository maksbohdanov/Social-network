using AutoMapper;
using BuisnessLogicLayer;
using BuisnessLogicLayer.Models.DTOs;
using DataAccessLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace SocialNetwork.Tests.Helpers
{
    internal class UnitTestHelper
    {
        #region TestData
        public static List<User> Users = new()
        {
            new User(){FirstName = "FirstName1", LastName = "LastName1", Email = "email1@test.com", City = "City1", BirthDate = DateTime.Today, Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")},
            new User(){FirstName = "FirstName2", LastName = "LastName2", Email = "email2@test.com", City = "City2", BirthDate = DateTime.Today, Id = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66")},
            new User(){FirstName = "FirstName3", LastName = "LastName3", Email = "email3@test.com", City = "City3", BirthDate = DateTime.Today, Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7")},
            new User(){FirstName = "FirstName4", LastName = "LastName4", Email = "email4@test.com", City = "City4", BirthDate = DateTime.Today, Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae8")}
        };

        public static List<UserChat> UserChats = new()
        {
            new UserChat() { User = Users[0]},
            new UserChat() { User = Users[1]},
            new UserChat() { User = Users[0]},
            new UserChat() { User = Users[2]},
         };

        public static List<Chat> Chats = new()
        {
            new Chat(){ Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728951e"), Users = UserChats.Take(2).ToList()},
            new Chat(){ Id = new Guid("30dd879c-ee2f-11db-8314-0800200c9a65"), Users = UserChats.TakeLast(2).ToList()}
        };

        public static List<Message> Messages = new()
        {
            new Message() { Text = "Hi!", Author = Users[0], Chat = Chats[0], Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")},
            new Message() { Text = "Hello!", Author = Users[1], Chat = Chats[0], Id = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66")},
            new Message() { Text = "How are you?", Author = Users[0], Chat = Chats[0], Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7")},
            new Message() { Text = "Text1", Author = Users[0], Chat = Chats[1], Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae1")},
            new Message() { Text = "Text2", Author = Users[2], Chat = Chats[1], Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae2")},
            new Message() { Text = "Text3", Author = Users[0], Chat = Chats[1], Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae3")},
        };
        #endregion

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

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


        public static void SeedData(SocialNetworkDbContext context)
        {            
            context.Users.AddRange(Users);
            context.Chats.AddRange(Chats); 
            context.Messages.AddRange(Messages);

            context.SaveChanges();
        }           
    }
}
