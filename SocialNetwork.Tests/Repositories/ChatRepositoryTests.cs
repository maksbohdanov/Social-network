using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using SocialNetwork.Tests.Helpers;

namespace SocialNetwork.Tests.Repositories
{
    internal class ChatRepositoryTests
    {
        static Guid[] guids = new Guid[] { new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), new Guid("30dd879c-ee2f-11db-8314-0800200c9a66") };
        private static List<Message> messages = new List<Message>()
        {
            new Message(){ Text = "Hi!",Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728952e")},
                new Message(){Text = "Hello!", Id = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66")},
                new Message(){Text = "How are you?", Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7")},
                new Message(){Text = "Text1", Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae1")},
                new Message(){Text = "Text2", Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae2")},
                new Message(){Text = "Text3", Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae3")},
        };
        private List<Chat> expectedChats = new List<Chat>()
        {
            new Chat(){
                Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728951e"),
                Messages = messages.Take(3).ToList(),
                Users = new List<UserChat>(new UserChat[2]) },
            new Chat(){ Id = new Guid("30dd879c-ee2f-11db-8314-0800200c9a65"),
                Messages = messages.TakeLast(3).ToList(),
                Users = new List < UserChat >(new UserChat[2])}
        };

        [TestCaseSource(nameof(guids))]
        public async Task ChatRepository_GetByIdAsync_ReturnsSingleValue(Guid id)
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var chatRepository = new ChatRepository(context);

            var chat = await chatRepository.GetByIdAsync(id.ToString());

            var expected = expectedChats.FirstOrDefault(x => x.Id == id);

            Assert.That(chat, Is.EqualTo(expected).Using(new ChatEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task ChatRepository_GetAllAsync_ReturnsAllValues()
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var chatRepository = new ChatRepository(context);
            var chats = await chatRepository.GetAllAsync();

            Assert.That(chats, Is.EqualTo(expectedChats).Using(new ChatEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [TestCase("Hi!")]
        public async Task ChatRepository_FindAsync_ReturnsCorrectValue(string text)
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var chatRepository = new ChatRepository(context);

            var chats = await chatRepository.FindAsync(x => x.Messages.Any(y => y.Text == text));

            var expected = expectedChats[0];

            Assert.That(chats.FirstOrDefault(), Is.EqualTo(expected).Using(new ChatEqualityComparer()), message: "FindAsync method works incorrect");
        }

        [Test]
        public async Task ChatRepository_AddAsync_AddsValueToDatabase()
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var chatRepository = new ChatRepository(context);
            var chat = new Chat();

            await chatRepository.CreateAsync(chat);
            await context.SaveChangesAsync();

            Assert.That(context.Chats.Count(), Is.EqualTo(expectedChats.Count + 1), message: "AddAsync method works incorrect");
        }


        [Test]
        public async Task ChatRepository_Update_DoesNotThrow()
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var chatRepository = new ChatRepository(context);
            var chat = expectedChats[0];
            chat.Id = Guid.Empty;

            Assert.DoesNotThrowAsync(() => chatRepository.UpdateAsync(chat));
        }

        [Test]
        public async Task ChatRepository_DeleteByIdAsync_DeletesEntity()
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var chatRepository = new ChatRepository(context);

            await chatRepository.DeleteByIdAsync("0f8fad5b-d9cb-469f-a165-70867728951e");
            await context.SaveChangesAsync();

            Assert.That(context.Chats.Count(), Is.EqualTo(expectedChats.Count - 1), message: "DeleteByIdAsync works incorrect");
        }
    }
}
