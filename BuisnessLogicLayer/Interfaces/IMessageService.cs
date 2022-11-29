using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;

namespace BuisnessLogicLayer.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto> GetByIdAsync(string messageId);
        Task<IEnumerable<MessageDto>> GetAllAsync(string chatId);
        Task<MessageDto> SendMessageAsync(MessageModel messageModel);
        Task<MessageDto> EditMessageAsync(string messageId, MessageModel model);
        Task<bool> DeleteMessageAsync(string messageId);
    }
}
