using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
    public class DataSeeder
    {      
        public static async Task Run(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider                                     
                                     .CreateScope();
            var provider = serviceScope.ServiceProvider;
            var context = provider.GetService<SocialNetworkDbContext>();
            var userManager = provider.GetRequiredService<UserManager<User>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await Initialize(userManager, roleManager);

            if (!context.Friendships.Any())
            {
                await Seed(context, userManager);
            }
            await context.SaveChangesAsync();
        }

        private static async Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            foreach (var role in new string[] {"User", "Admin" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
            if (!adminUsers.Any())
            {
                var adminUser = new User()
                {
                    FirstName = "Admin",
                    LastName = "Social Network",
                    Email = "admin@admin.com",
                    UserName = "admin"
                };

                var result = await userManager.CreateAsync(adminUser, "Adm1n123Passw0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "User");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private static async Task Seed(SocialNetworkDbContext context, UserManager<User> userManager)
        {
            var users = new List<User> 
            {
                new User
                {
                    FirstName = "The",
                    LastName = "Narrator",
                    Email = "user1@user.com",
                    UserName= "the.narrator",
                    City = "Ternopil",
                    BirthDate = new DateTime(1980, 1, 12)
                },
                 new User
                {
                    FirstName = "Tyler",
                    LastName = "Durden",
                    Email = "user2@user.com",
                    UserName= "tyler.durden",
                    City = "Ternopil",
                    BirthDate = new DateTime(1980, 2, 23)
                }
            };
            for(var i = 0; i < users.Count; i++) 
            {
                await userManager.CreateAsync(users[i], $"Password{i+1}");
                await userManager.AddToRoleAsync(users[i], "User");
            }

            var user1 = await userManager.FindByNameAsync("the.narrator");
            var user2 = await userManager.FindByNameAsync("tyler.durden");
            var friendship = new Friendship()
            {
                User = user1,
                Friend = user2,
                IsAccepted = true
            };
            context.Friendships.Add(friendship);
        }
    }
}
