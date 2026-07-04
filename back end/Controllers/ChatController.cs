using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School_Project_API.DTO;
using School_Project_API.Services.Interfaces;
using SchoolManagementSystem.API.Services;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IGeminiService _geminiService;

        public ChatController(IGeminiService claudeService)
        {
            _geminiService = claudeService;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequestDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message cannot be empty.");

            try
            {
                var result = await _geminiService.SendMessageAsync(request.Message);
                return Ok(new { response = result });
            }
            catch (Exception ex)
            {
                return StatusCode(502, $"AI service error: {ex.Message}");
            }
        }
    }
}
