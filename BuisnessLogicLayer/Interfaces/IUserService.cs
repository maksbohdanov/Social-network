using BuisnessLogicLayer.Models.DTOs;
using BuisnessLogicLayer.Models;

namespace BuisnessLogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> RegisterAsync(UserRegistrationModel registerModel);
        Task<string> LoginAsync(LoginModel loginModel);
        Task<UserDto> UpdateAsync(UserDto updateModel);
        Task<bool> DeleteUserAsync(string userId);
    }
}
