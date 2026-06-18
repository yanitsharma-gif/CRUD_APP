using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models;
using MediatR;
using Practice.Commands;
namespace Practice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;

        public ProductsController(AppDbContext context,IMediator mediator)
        {
            _context = context;
            _mediator=mediator;
        }



        [HttpGet]
        public async Task<IActionResult> GetProducts(
            CancellationToken cancellationToken)
        {
            var products = await _context.Products
                .ToListAsync(cancellationToken);

            return Ok(products);
        }





        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCommand(
                id
                ),cancellationToken);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }






        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            Product product,
            CancellationToken cancellationToken)
        {


            var result= await _mediator.Send(new CreateCommand(product),cancellationToken);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }







        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            Product updatedProduct,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send
                (
                new UpdateCommand(
                    id, updatedProduct
                ), cancellationToken
                );
            if (!result.Success) {

                return BadRequest(result);
            }
            return Ok(result);

        }





        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(

               new DeleteCommand(
                   id
                   )
               ,cancellationToken
                );

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}