using Application.Commands.ProductCommands.Create;
using Application.Commands.ProductCommands.Delete;
using Application.Commands.ProductCommands.Update;
using Application.Queries.ProductQueries;
using Application.Queries.ProductQueries.GetByList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyDreamShop.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISender _sender;
        public ProductController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var id = await _sender.Send(command);
            return Ok(new { ID = id, Message = "product was created" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateProductCommand command)
        {

            var result = await _sender.Send(command);
            return Ok("Product updated successfully.");
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(DeleteProductCommand command)
        {
            var result = await _sender.Send(command);
            return Ok("Product deleted successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetProductByIdQuery
            {
                Id = id
            }, cancellationToken);

            return Ok(result);
        }

        [HttpGet("Get product list")]

        public async Task<IActionResult> GetsssByList( CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new ProductByListQuery { }, cancellationToken);
            return Ok(result);

        }

    }
}
