using BuisnessLogicLayer.Exceptions;
using BuisnessLogicLayer.Interfaces;
using BuisnessLogicLayer.Models.DTOs;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using FluentAssertions;
using Moq;
using SocialNetwork.Tests.Helpers;
using DataAccessLayer.Entities;

namespace SocialNetwork.Tests.Services
{
    [TestFixture]
    internal class ChatServiceTests
    {
        private IChatService _chatService;
        private Mock<IUnitOfWork> _unitOfWork;
        private List<Chat> _mockedChats;

        [SetUp]
        public void Setup() 
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _chatService = new ChatService(_unitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            _mockedChats = new()
            {
                new Chat(){
                    Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728951e"),
                    Users = new List<UserChat>()
                    {
                        new UserChat() { UserId = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae8")}
                    },
                    Messages = new List<Message>(){new Message() { Id = new Guid()} }
                }
            };
        }

        [Test]
        public async Task ChatService_GetByIdAsync_ReturnsChatModel()
        {
            var expected = new ChatDto()
            {
                Id = "0f8fad5b-d9cb-469f-a165-70867728951e",
                Messages = new string[1],
                Users = new string[1]
            };
            _unitOfWork.Setup(x => x.Chats.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockedChats[0]);

            var actual = await _chatService.GetByIdAsync(string.Empty);

            Assert.That(actual.Id == expected.Id &&
                actual.Users.Count == expected.Users.Count &&
                actual.Messages.Count == expected.Messages.Count);
        }

        [Test]
        public async Task ChatService_GetByIdAsync_ThrowsNotFoundException()
        {
            _unitOfWork.Setup(x => x.Chats.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Chat>());

            Func<Task> act = async () => await _chatService.GetByIdAsync(It.IsAny<string>());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ChatService_GetAllAsync_ReturnsAllChats()
        {
            _unitOfWork.Setup(x => x.Chats.FindAsync(It.IsAny<Func<Chat, bool>>()))
                .ReturnsAsync(It.IsAny<IEnumerable<Chat>>());

            var chats = await _chatService.GetAllAsync("7c9e6679-7425-40de-944b-e07fc1f90ae8");
            _unitOfWork.VerifyAll();
        }

        [Test]
        public async Task ChatService_CreateChatAsyncc_CreatesChat()
        {
            var model = new NewChatModel
            {
                FirstUserId = Guid.NewGuid().ToString(),
                SecondUserId = Guid.NewGuid().ToString(),
            };

            _unitOfWork.Setup(x => x.Chats.CreateAsync(It.IsAny<Chat>()))
                .ReturnsAsync(true);
            _unitOfWork.Setup(x => x.Chats.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockedChats[0]);

            await _chatService.CreateChatAsync(model);
            _unitOfWork.Verify(x => x.Chats.CreateAsync(It.IsAny<Chat>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));
        }

        [TestCase("")]
        public async Task ChatService_DeleteMessageAsync_DeletesMessage(string id)
        {
            _unitOfWork.Setup(x => x.Chats.DeleteByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            await _chatService.DeleteChatAsync(id);

            _unitOfWork.Verify(x => x.Chats.DeleteByIdAsync(id), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase("")]
        public async Task ChatService_DeleteMessageAsync_ThrowsNotFoundException(string id)
        {
            _unitOfWork.Setup(x => x.Chats.DeleteByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _chatService.DeleteChatAsync(id);

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
