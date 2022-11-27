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
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
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
