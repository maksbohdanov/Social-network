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
            //var context = provider.GetService<SocialNetworkDbContext>();
            var userManager = provider.GetRequiredService<UserManager<User>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await Initialize(userManager, roleManager);

            //if (!context.Products.Any())
            //{
            //    // Seed Other entities Here
            //}
            //await context.SaveChangesAsync();
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
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private async Task Seed(SocialNetworkDbContext context)
        {
            await context.SaveChangesAsync();
        }
    }
}
