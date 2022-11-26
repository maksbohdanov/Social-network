using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;

namespace BuisnessLogicLayer.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto> GetByIdAsync(string messageId);
        Task<IEnumerable<MessageDto>> GetAllAsync(string chatId);
        Task<MessageDto> SendMessageAsync(string userId, string chatId, MessageModel messageModel);
        Task EditMessageAsync(string messageId, MessageModel model);
        Task<bool> DeleteMessageAsync(string messageId);
    }
}
