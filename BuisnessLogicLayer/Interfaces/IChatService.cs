using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;

namespace BuisnessLogicLayer.Interfaces
{
    public interface IChatService
    {
        Task<ChatDto> GetByIdAsync(string chatId);
        Task<IEnumerable<ChatDto>> GetAllAsync(string userId);
        Task<ChatDto> CreateChatAsync(NewChatModel chatModel);
        Task<bool> CheckIfChatExists(NewChatModel chatModel);
        Task<bool> DeleteChatAsync(string chatId);
    }
}
