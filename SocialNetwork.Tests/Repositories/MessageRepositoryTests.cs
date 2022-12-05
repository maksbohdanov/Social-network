using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using SocialNetwork.Tests.Helpers;

namespace SocialNetwork.Tests.Repositories
{
    [TestFixture]
    internal class MessageRepositoryTests
    {       
        static Guid[] guids = new Guid[] {new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"), new Guid("30dd879c-ee2f-11db-8314-0800200c9a66") };
        private List<Message> expectedMessages = new List<Message>()
        {
             new Message(){ 
                 Text = "Hi!",
                 AuthorId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                 ChatId = new Guid("0f8fad5b-d9cb-469f-a165-70867728951e"),
                 Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")},
             new Message(){
                 Text = "Hello!",
                 AuthorId = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66"),
                 ChatId = new Guid("0f8fad5b-d9cb-469f-a165-70867728951e"),
                 Id = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66")}             
        };      
        
        [TestCaseSource(nameof(guids))]
        public async Task MessageRepository_GetByIdAsync_ReturnsSingleValue(Guid id)
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var messageRepository = new MessageRepository(context);

            var user = await messageRepository.GetByIdAsync(id.ToString());

            var expected = expectedMessages.FirstOrDefault(x => x.Id == id);

            Assert.That(user, Is.EqualTo(expected).Using(new MessageEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }

        [Test]
        public async Task MessageRepository_GetAllAsync_ReturnsAllValues()
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var messageRepository = new MessageRepository(context);
            var messages = await messageRepository.GetAllAsync();

            Assert.That(messages.Take(2), Is.EqualTo(expectedMessages).Using(new MessageEqualityComparer()), message: "GetAllAsync method works incorrect");
        }

        [TestCase("Hello!")]
        public async Task MessageRepository_FindAsync_ReturnsCorrectValue(string text)
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var messageRepository = new MessageRepository(context);

            var users = await messageRepository.FindAsync(x => x.Text == text);

            var expected = expectedMessages[1];

            Assert.That(users.FirstOrDefault(), Is.EqualTo(expected).Using(new MessageEqualityComparer()), message: "FindAsync method works incorrect");
        }

        [Test]
        public async Task MessageRepository_AddAsync_AddsValueToDatabase()
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var messageRepository = new MessageRepository(context);
            var message = new Message
            {
                Text = "New text"
            };

            await messageRepository.CreateAsync(message);
            await context.SaveChangesAsync();

            Assert.That(context.Messages.Count()-4, Is.EqualTo(expectedMessages.Count + 1), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task MessageRepository_Update_DoesNotThrow()
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var messageRepository = new MessageRepository(context);
            var message = expectedMessages[0];
            message.Text = "Edited text";

            Assert.DoesNotThrowAsync(() =>  messageRepository.UpdateAsync(message));
        }

        [Test]
        public async Task MessageRepository_DeleteByIdAsync_DeletesEntity()
        {
            using var context = new SocialNetworkDbContext(RepositoryHelper.GetForumDbOptions());

            var messageRepository = new MessageRepository(context);

            await messageRepository.DeleteByIdAsync("0f8fad5b-d9cb-469f-a165-70867728950e");
            await context.SaveChangesAsync();

            Assert.That(context.Messages.Count()-4, Is.EqualTo(expectedMessages.Count - 1), message: "DeleteByIdAsync works incorrect");
        }
    }
}
