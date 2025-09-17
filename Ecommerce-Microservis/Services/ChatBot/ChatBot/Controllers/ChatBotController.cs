using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {

    }
}

public class ChatMessageDto
{
    public required string Message { get; set; }
}
