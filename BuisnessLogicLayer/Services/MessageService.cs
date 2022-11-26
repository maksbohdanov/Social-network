using AutoMapper;
using BuisnessLogicLayer.Exceptions;
using BuisnessLogicLayer.Interfaces;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BuisnessLogicLayer.Services
{
    public class MessageService: IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MessageDto> GetByIdAsync(string messageId)
        {
            var result = await _unitOfWork.Messages.GetByIdAsync(messageId);
            if(result == null) 
            {
                throw new NotFoundException("Message with specified id was not found");
            }
            return _mapper.Map<MessageDto>(result);
        }

        public async Task<IEnumerable<MessageDto>> GetAllAsync(string chatId)
        {
            var messages = await _unitOfWork.Messages
                .FindAsync(m => m.ChatId.ToString() == chatId);

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<MessageDto> SendMessageAsync(string userId, string chatId, MessageModel messageModel)
        {
            var message = _mapper.Map<Message>(messageModel);
            await _unitOfWork.Messages.CreateAsync(message);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MessageDto>(message);
        }

        public async Task EditMessageAsync(string messageId, MessageModel model)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(messageId);
            if (message == null)
            {
                throw new NotFoundException("Message with specified id was not found");
            }
            message.Text= model.Text;
            await _unitOfWork.Messages.UpdateAsync(message);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteMessageAsync(string messageId)
        {
            var result = await _unitOfWork.Messages.DeleteByIdAsync(messageId);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
