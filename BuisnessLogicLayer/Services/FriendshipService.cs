using AutoMapper;
using BuisnessLogicLayer.Exceptions;
using BuisnessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BuisnessLogicLayer.Services
{
    public class FriendshipService: IFriendshipService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FriendshipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Friendship> GetByIdAsync(string id)
        {
            var result = await _unitOfWork.Friendships.GetByIdAsync(id);
            if (result == null)
            {
                throw new NotFoundException("Specified friendship was not found");
            }
            return result;
        }

        public async Task<Friendship> FindByUsersAsync(string userId, string friendId)
        {
            var friendship = (await _unitOfWork.Friendships
                    .FindAsync(x => (x.UserId.ToString() == userId && x.FriendId.ToString() == friendId) ||
                              (x.UserId.ToString() == friendId && x.FriendId.ToString() == userId)))
                .FirstOrDefault();

            if (friendship == null)
                throw new NotFoundException("Specified friendship was not found");

            return friendship;
        }
        
        public async Task<IEnumerable<Friendship>> GetRequestsAsync(string userId)
        {
            var friendships = await _unitOfWork.Friendships
                .FindAsync(x => x.UserId.ToString() == userId &&
                                x.IsAccepted == false);
            return friendships;
        }

        public async Task<IEnumerable<Friendship>> GetFriendsAsync(string userId)
        {
            var friendships = await _unitOfWork.Friendships
                    .FindAsync(x => x.IsAccepted && (x.UserId.ToString() == userId ||
                               x.FriendId.ToString() == userId));
            return friendships;
        }

        public async Task<Friendship> CreateFriendshipAsync(User user, User friend)
        {
            var friendship = new Friendship
            {
                User = user,
                Friend = friend
            };

            await _unitOfWork.Friendships.CreateAsync(friendship);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(friendship.Id.ToString());
        }

        public async Task<bool> DeleteFriendshipAsync(string id)
        {
            var result = await _unitOfWork.Friendships.DeleteByIdAsync(id);
            if (!result)
                throw new NotFoundException("Specified friendship was not found");
            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}
