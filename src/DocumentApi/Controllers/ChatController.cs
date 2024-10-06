using DocumentApi.DTOs;
using DocumentApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService chatService;

    public ChatController(IChatService chatService)
    {
        this.chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Ask([FromBody] QuestionRequestDto question)
    {
        var response = await chatService.AskAsync(question.Question);

        return Ok(response);
    }
}