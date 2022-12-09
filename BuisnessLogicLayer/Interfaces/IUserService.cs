using BuisnessLogicLayer.Models.DTOs;
using BuisnessLogicLayer.Models;

namespace BuisnessLogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<IEnumerable<UserDto>> FindByFilterAsync(string filter, string userId);
        Task<UserDto> RegisterAsync(UserRegistrationModel registerModel);
        Task<string> LoginAsync(LoginModel loginModel);
        Task<UserDto> UpdateAsync(UserDto updateModel);
        Task<bool> DeleteUserAsync(string userId);
        Task AddToFriendsAsync(string userId, string friendId);
        Task ApproveFriendshipRequestAsync(string friendshipId, bool answer);
        Task<IEnumerable<FriendshipDto>> GetFriendshipRequestsAsync(string userId);
        Task<IEnumerable<UserDto>> GetFriendsAsync(string userId);
    }
}
