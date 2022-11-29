using BuisnessLogicLayer.Interfaces;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatsController(IChatService chatService)
        {
            _chatService= chatService;
        }

        [HttpGet("{chatId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetById(string chatId)
        {
            var chat = await _chatService.GetByIdAsync(chatId);

            return Ok(chat);
        }

        [HttpGet("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatDto>))]
        public async Task<IActionResult> GetUserChats(string userId)
        {
            var chats = await _chatService.GetAllAsync(userId);

            return Ok(chats);
        }       

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ChatDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> CreateChat([FromBody] NewChatModel newChat)
        {
            var createdChat = await _chatService.CreateChatAsync(newChat);

            return CreatedAtAction(nameof(GetById), new { id = createdChat.Id }, createdChat);
        }


        [HttpDelete("{chatId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> DeleteById(string chatId)
        {
            var result = await _chatService.DeleteChatAsync(chatId);

            return Ok(result);
        }
    }
}
