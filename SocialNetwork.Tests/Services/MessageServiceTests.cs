using BuisnessLogicLayer.Exceptions;
using BuisnessLogicLayer.Interfaces;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;
using BuisnessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using FluentAssertions;
using Moq;
using SocialNetwork.Tests.Helpers;

namespace SocialNetwork.Tests.Services
{
    [TestFixture]
    internal class MessageServiceTests
    {
        private IMessageService _messageService;
        private Mock<IUnitOfWork> _unitOfWork;
        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _messageService = new MessageService(_unitOfWork.Object, UnitTestHelper.CreateMapperProfile());
        }

        [Test]
        public async Task MessageService_GetByIdAsync_ReturnsChatModel()
        {
            var expected = new MessageDto()
            {
                Id = "0f8fad5b-d9cb-469f-a165-70867728950e",
                Text = "Hi!",
                AuthorId = "0f8fad5b-d9cb-469f-a165-70867728950e",
                ChatId = "0f8fad5b-d9cb-469f-a165-70867728951e"
            };
            _unitOfWork.Setup(x => x.Messages.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestHelper.Messages[0]);

            var actual = await _messageService.GetByIdAsync(string.Empty);

            actual.Should().BeEquivalentTo(expected, o => o
                .Excluding(x => x.TimeCreated));
        }

        [Test]
        public async Task MessageService_GetByIdAsync_ThrowsNotFoundException()
        {
            _unitOfWork.Setup(x => x.Messages.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Message>());

            Func<Task> act = async () => await _messageService.GetByIdAsync(It.IsAny<string>());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task MessageService_GetAllAsync_ReturnsAllMessages()
        {
            _unitOfWork.Setup(x => x.Messages.FindAsync(It.IsAny<Func<Message, bool>>()))
                .ReturnsAsync(UnitTestHelper.Messages
                .Where(x => x.Chat.Id == new Guid("0f8fad5b-d9cb-469f-a165-70867728951e")));

            var messages = await _messageService.GetAllAsync("0f8fad5b-d9cb-469f-a165-70867728951e");
            Assert.That(messages.Count, Is.EqualTo(3));
            _unitOfWork.VerifyAll();
        }

        [Test]
        public async Task MessageService_SendMessageAsync_CreatesMessage()
        {
            var model = new MessageModel {
                Text = "Hello",
                AuthorId = Guid.NewGuid().ToString(),
                ChatId = Guid.NewGuid().ToString() 
            };

            _unitOfWork.Setup(x => x.Messages.CreateAsync(It.IsAny<Message>()))
                .ReturnsAsync(true);

            await _messageService.SendMessageAsync(model);
            _unitOfWork.Verify(x => x.Messages.CreateAsync(It.IsAny<Message>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase("")]
        public async Task MessageService_EditMessageAsync_EditsMessage(string id)
        {
            var model = new MessageModel
            {
                Text = "Hello",
                AuthorId = Guid.NewGuid().ToString(),
                ChatId = Guid.NewGuid().ToString()
            };
            _unitOfWork.Setup(x => x.Messages.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestHelper.Messages[1]);
            _unitOfWork.Setup(x => x.Messages.UpdateAsync(It.IsAny<Message>()))
                .ReturnsAsync(true);

            await _messageService.EditMessageAsync(id, model);

            _unitOfWork.Verify(x => x.Messages.UpdateAsync(It.IsAny<Message>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase("")]
        public async Task MessageService_EditMessageAsync_ThrowsNotFoundException(string id)
        {
            var model = new MessageModel
            {
                Text = "Hello",
                AuthorId = Guid.NewGuid().ToString(),
                ChatId = Guid.NewGuid().ToString()
            };
            _unitOfWork.Setup(x => x.Messages.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Message>());

            Func<Task> act = async () => await _messageService.EditMessageAsync(id, model);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [TestCase("")]
        public async Task MessageService_DeleteMessageAsync_DeletesMessage(string id)
        {
            _unitOfWork.Setup(x => x.Messages.DeleteByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            await _messageService.DeleteMessageAsync(id);

            _unitOfWork.Verify(x => x.Messages.DeleteByIdAsync(id), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [TestCase("")]
        public async Task MessageService_DeleteMessageAsync_ThrowsNotFoundException(string id)
        {
            _unitOfWork.Setup(x => x.Messages.DeleteByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _messageService.DeleteMessageAsync(id);

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
