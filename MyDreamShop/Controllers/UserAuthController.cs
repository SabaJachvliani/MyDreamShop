using Application.AuthUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyDreamShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAuthController : ControllerBase
    {
        private readonly ISender _sender;

        public UserAuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var userId = await _sender.Send(command, cancellationToken);
            return Ok(new { Id = userId, Message = "User registered successfully" });
        }
    }
}
