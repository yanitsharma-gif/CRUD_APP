using Practice.Data;
using Practice.Models;
using Microsoft.EntityFrameworkCore;
namespace Practice.Repositories
{
    public class GetAllRepo
    {
      private readonly AppDbContext _context;

        public GetAllRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAll()
        {
            try
            {
                var products = await _context.Products.ToListAsync();

                return products;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
