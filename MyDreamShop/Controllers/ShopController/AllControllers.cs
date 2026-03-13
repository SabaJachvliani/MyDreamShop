using Application.Commands.ProductCommands.Create;
using Application.Commands.ProductCommands.Delete;
using Application.Commands.ProductCommands.Update;
using Application.Commands.ReviewCommands.Create;
using Application.Commands.ReviewCommands.Delete;
using Application.Commands.ReviewCommands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyDreamShop.Controllers.ShopController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllControllers : ControllerBase
    {
        private readonly ISender _sender;

        public AllControllers(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var id = await _sender.Send(command);
            return Ok( new { ID = id, Message = "product was created"});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateProductCommand command)
        {
           
            var result = await _sender.Send(command);            
            return Ok("Product updated successfully.");
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete( DeleteProductCommand command)
        {
            var result = await _sender.Send(command);
            return Ok("Product deleted successfully.");
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
