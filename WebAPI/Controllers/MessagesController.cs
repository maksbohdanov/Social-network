using BuisnessLogicLayer.Interfaces;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessagesController(IMessageService messageService)
        {
            _messageService= messageService;
        }

        [HttpGet("{messageId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetById(string messageId)
        {
            var message = await _messageService.GetByIdAsync(messageId);

            return Ok(message);
        }

        [HttpGet("chats/{chatId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MessageDto>))]
        public async Task<IActionResult> GetAll(string chatId)
        {
            var messages = await _messageService.GetAllAsync(chatId);

            return Ok(messages);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MessageDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> SendMessage([FromBody] MessageModel model)
        {
            var sentMessage = await _messageService.SendMessageAsync(model);

            return CreatedAtAction(nameof(SendMessage), new { id = sentMessage.Id }, sentMessage);
        }

        [HttpPut("{messageId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> EditMessage(string messageId, [FromBody] MessageModel model)
        {
            var message = await _messageService.EditMessageAsync(messageId, model);

            return Ok(message);
        }

        [HttpDelete("{messageId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> DeleteById(string messageId)
        {
            var result = await _messageService.DeleteMessageAsync(messageId);

            return Ok(result);
        }
    }
}
