using AutoMapper;
using BuisnessLogicLayer.Exceptions;
using BuisnessLogicLayer.Interfaces;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BuisnessLogicLayer.Services
{
    public class ChatService: IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }        

        public async Task<ChatDto> GetByIdAsync(string chatId)
        {
            var result = await _unitOfWork.Chats.GetByIdAsync(chatId);
            if (result == null)
            {
                throw new NotFoundException("Chat with specified id was not found");
            }
            return _mapper.Map<ChatDto>(result);
        }

        public async Task<IEnumerable<ChatDto>> GetAllAsync(string userId)
        {
            var chats = await _unitOfWork.Chats
               .FindAsync(c => c.Users
                 .Any(x => x.UserId.ToString() == userId));

            return _mapper.Map<IEnumerable<ChatDto>>(chats);
        }

        public async Task<ChatDto> CreateChatAsync(NewChatModel chatModel)
        {
            var chat = new Chat();
            await _unitOfWork.Chats.CreateAsync(chat);

            var firstUser = new UserChat
            {
                ChatId = chat.Id,
                UserId = new Guid(chatModel.FirstUserId)
            };
            var secondUser = new UserChat
            {
                ChatId = chat.Id,
                UserId = new Guid(chatModel.SecondUserId)
            };

            chat.Users.Add(firstUser);
            chat.Users.Add(secondUser);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ChatDto>(chat);
        }

        public async Task<bool> DeleteChatAsync(string chatId)
        {
            var result = await _unitOfWork.Chats.DeleteByIdAsync(chatId);
            if (!result)
                throw new NotFoundException("Chat with specified id was not found");
            await _unitOfWork.SaveChangesAsync();
            
            return result;
        }
    }
}
