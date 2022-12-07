using AutoMapper;
using BuisnessLogicLayer.Exceptions;
using BuisnessLogicLayer.Interfaces;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BuisnessLogicLayer.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IFriendshipService _friendshipService;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager,
                IFriendshipService friendshipService ,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _friendshipService = friendshipService;
            _configuration = configuration;
        }        

        public async Task<UserDto> GetByIdAsync(string userId)
        {
            var result = await _userManager.FindByIdAsync(userId);

            if (result == null)
            {
                throw new NotFoundException("User with specified id was not found");
            }
            return _mapper.Map<UserDto>(result);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> RegisterAsync(UserRegistrationModel registerModel)
        {
            var user = _mapper.Map<User>(registerModel);
            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if(!result.Succeeded)
            {
                var message = string.Join(" ", result.Errors.Select(e => e.Description));
                throw new RegisterUserException(message);
            }
            await _userManager.AddToRoleAsync(user, "User");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
                throw new NotFoundException("User with specified email was not found");                

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if(!passwordCorrect)
                 throw new LoginException("Wrong password.");

            return await GenerateToken(user);
        }      
        
        public async Task<UserDto> UpdateAsync(UserDto updateModel)
        {
            var user = await _userManager.FindByIdAsync(updateModel.Id);
            if (user == null)
                throw new NotFoundException("User with specified id was not found");

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var message = string.Join(" ", result.Errors.Select(e => e.Description));
                throw new UpdateUserException(message);
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var result = await _unitOfWork.Users.DeleteByIdAsync(userId);
            if (!result)
                throw new NotFoundException("User with specified id was not found");
            await _unitOfWork.SaveChangesAsync();
            
            return result;
        }

        public async Task AddToFriendsAsync(string userId, string friendId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var friend = await _userManager.FindByIdAsync(friendId);
            if (user == null || friend == null)
                throw new NotFoundException("User with specified id was not found");

            _ = await _friendshipService.FindByUsersAsync(userId, friendId)
                        ?? await _friendshipService.CreateFriendshipAsync(user, friend);
        }

        public async Task ApproveFriendshipRequestAsync(string friendshipId, bool answer)
        {
            if (!answer)
                await _friendshipService.DeleteFriendshipAsync(friendshipId);
            else
            {
                var friendship = await _unitOfWork.Friendships.GetByIdAsync(friendshipId);
                friendship.IsAccepted = answer;
                friendship.CreationDate = DateTime.UtcNow;
                await _unitOfWork.Friendships.UpdateAsync(friendship);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<FriendshipDto>> GetFriendshipRequestsAsync(string userId)
        {
            var friendships = await _friendshipService.GetRequestsAsync(userId);

            return _mapper.Map<IEnumerable<FriendshipDto>>(friendships);
        }

        public async Task<IEnumerable<UserDto>> GetFriendsAsync(string userId)
        {
            var friendships = await _friendshipService.GetFriendsAsync(userId);

            var friends = friendships
                .Where(x => x.UserId.ToString() != userId)
                .Select(x => x.User)
                .Concat(friendships
                    .Where(x => x.FriendId.ToString() != userId)
                    .Select(x => x.Friend));

            return _mapper.Map<IEnumerable<UserDto>>(friends);
        }

        private async Task<string> GenerateToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)                
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
