using DataAccessLayer.Entities;

namespace BuisnessLogicLayer.Interfaces
{
    public interface IFriendshipService
    {
        Task<Friendship> GetByIdAsync(string id);
        Task<Friendship?> FindByUsersAsync(string userId, string friendId);
        Task<IEnumerable<Friendship>> GetRequestsAsync(string userId);
        Task<IEnumerable<Friendship>> GetFriendsAsync(string userId);
        Task<Friendship> CreateFriendshipAsync(User user, User friend);
        Task<bool> CheckIfFriendshipExists(string userId, string friendId);
        Task<bool> DeleteFriendshipAsync(string id);
    }
}
