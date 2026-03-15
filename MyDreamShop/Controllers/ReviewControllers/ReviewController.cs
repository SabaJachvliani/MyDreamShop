using Application.Commands.ReviewCommands.Create;
using Application.Commands.ReviewCommands.Delete;
using Application.Commands.ReviewCommands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyDreamShop.Controllers.ReviewControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ISender _sender;

        public ReviewController(ISender sender)
        {
            _sender = sender;
        }
                
        [HttpPost("Create Review")]
        public async Task<IActionResult> Create([FromBody] CreateReviewCommand command)
        {
            var id = await _sender.Send(command);
            return Ok(new { Id = id, Message = "Review created successfully." });
        }

        [HttpPut("{id}update")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch.");

            var result = await _sender.Send(command);

            if (!result)
                return NotFound("Review not found.");

            return Ok("Review updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sender.Send(new DeleteReviewCommand { Id = id });

            if (!result)
                return NotFound("Review not found.");

            return Ok("Review deleted successfully.");
        }
    }
}
