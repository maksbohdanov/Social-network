using BuisnessLogicLayer.Models.DTOs;

namespace BuisnessLogicLayer.Interfaces
{
    public interface IChatService
    {
        Task<ChatDto> GetByIdAsync(string chatId);
        Task<IEnumerable<ChatDto>> GetAllAsync(string userId);
        Task<ChatDto> CreateChatAsync(string firstUserId, string secondUserId);
        Task<bool> DeleteChatAsync(string chatId);
    }
}
