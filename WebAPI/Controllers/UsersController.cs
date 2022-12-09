using BuisnessLogicLayer.Interfaces;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebAPI.Models;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) 
        {
            _userService= userService;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetById(string userId)
        {
            var user = await _userService.GetByIdAsync(userId);

            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
        public async Task<IActionResult> GetAll()
        {
            var users =  await _userService.GetAllAsync();

            return Ok(users);
        }

        [HttpGet("filter/{filter}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
        public async Task<IActionResult> FindByFilterAll(string filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = await _userService.FindByFilterAsync(filter, userId);
            
            return Ok(users);
        }


        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            var user = await _userService.RegisterAsync(model);

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _userService.LoginAsync(model);

            return Ok(token);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> Update([FromBody] UserDto model)
        {
            var user = await _userService.UpdateAsync(model);

            return Ok(user);
        }

        [HttpGet("{userId}/friends")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetFriends(string userId)
        {
            var friends = await _userService.GetFriendsAsync(userId);

            return Ok(friends);
        }

        [HttpPut("{userId}/{friendId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> AddToFriends(string userId, string friendId)
        {
            await _userService.AddToFriendsAsync(userId, friendId);

            return Ok();
        }

        [HttpPut("friendship/{friendshipId}/{answer}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> ApproveFriendship(string friendshipId, bool answer)
        {
            await _userService.ApproveFriendshipRequestAsync(friendshipId, answer);

            return Ok();
        }

        [HttpGet("{userId}/requests")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FriendshipDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetFriendshipRequests(string userId)
        {
            var requests = await _userService.GetFriendshipRequestsAsync(userId);

            return Ok(requests);
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> DeleteById(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);

            return Ok(result);
        }
    }
}
