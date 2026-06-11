using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models;

namespace Practice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
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
            var product = await _context.Products
                .FindAsync(new object[] { id }, cancellationToken);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(
            Product product,
            CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(
                product,
                cancellationToken);

            await _context.SaveChangesAsync(
                cancellationToken);

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            Product updatedProduct,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FindAsync(new object[] { id }, cancellationToken);

            if (product == null)
                return NotFound();

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            await _context.SaveChangesAsync(
                cancellationToken);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(
            int id,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FindAsync(new object[] { id }, cancellationToken);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);

            await _context.SaveChangesAsync(
                cancellationToken);

            return NoContent();
        }
    }
}