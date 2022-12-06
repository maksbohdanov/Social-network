using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using FluentAssertions;
using SocialNetwork.Tests.Helpers;

namespace SocialNetwork.Tests.Repositories
{
    [TestFixture]
    internal class UserRepositoryTests
    {       
        static Guid[] guids = new Guid[] {new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae8")};
        private List<User> expectedUsers = new List<User>()
        {
            new User(){FirstName = "FirstName1", LastName = "LastName1", Email ="email1@test.com", City = "City1", BirthDate = DateTime.Today, Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")},
            new User(){FirstName = "FirstName2", LastName = "LastName2", Email ="email2@test.com", City = "City2", BirthDate = DateTime.Today, Id = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66")},
            new User(){FirstName = "FirstName3", LastName = "LastName3", Email = "email3@test.com", City = "City3", BirthDate = DateTime.Today, Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7")},
            new User(){FirstName = "FirstName4", LastName = "LastName4", Email = "email4@test.com", City = "City4", BirthDate = DateTime.Today, Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae8")}
        };      
        
        [TestCaseSource(nameof(guids))]
        public async Task UserRepository_GetByIdAsync_ReturnsSingleValue(Guid id)
        {
            using var context = new SocialNetworkDbContext(UnitTestHelper.GetForumDbOptions());

            var userRepository = new UserRepository(context);

            var user = await userRepository.GetByIdAsync(id.ToString());

            var expected = expectedUsers.FirstOrDefault(x => x.Id == id);

            Assert.That(user, Is.EqualTo(expected).Using(new UserEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task UserRepository_GetAllAsync_ReturnsAllValues()
        {
            using var context = new SocialNetworkDbContext(UnitTestHelper.GetForumDbOptions());

            var userRepository = new UserRepository(context);
            var users = await userRepository.GetAllAsync();

            Assert.That(users, Is.EqualTo(expectedUsers).Using(new UserEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [TestCase("City1")]
        public async Task UserRepository_FindAsync_ReturnsCorrectValue(string city)
        {
            using var context = new SocialNetworkDbContext(UnitTestHelper.GetForumDbOptions());

            var userRepository = new UserRepository(context);

            var users = await userRepository.FindAsync(x => x.City == city);

            var expected = expectedUsers[0];

            Assert.That(users.FirstOrDefault(), Is.EqualTo(expected).Using(new UserEqualityComparer()), message: "FindAsync method works incorrect");
        }

        [Test]
        public async Task UserRepository_AddAsync_AddsValueToDatabase()
        {
            using var context = new SocialNetworkDbContext(UnitTestHelper.GetForumDbOptions());

            var userRepository = new UserRepository(context);
            var user = new User
            {
                Email = "email6@mail.com"
            };

            await userRepository.CreateAsync(user);
            await context.SaveChangesAsync();

            Assert.That(context.Users.Count(), Is.EqualTo(expectedUsers.Count + 1), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task UserRepository_Update_DoesNotThrow()
        {
            using var context = new SocialNetworkDbContext(UnitTestHelper.GetForumDbOptions());

            var userRepository = new UserRepository(context);
            var user = expectedUsers[0];
            user.FirstName = "New FirstName";


            Func<Task> act = async () => await userRepository.UpdateAsync(user);

            await act.Should().NotThrowAsync();
        }

        [Test]
        public async Task UserRepository_DeleteByIdAsync_DeletesEntity()
        {
            using var context = new SocialNetworkDbContext(UnitTestHelper.GetForumDbOptions());

            var userRepository = new UserRepository(context);

            await userRepository.DeleteByIdAsync("0f8fad5b-d9cb-469f-a165-70867728950e");
            await context.SaveChangesAsync();

            Assert.That(context.Users.Count(), Is.EqualTo(expectedUsers.Count - 1), message: "DeleteByIdAsync works incorrect");
        }
    }
}
