using Practice.Data;
using Practice.Models;

namespace Practice.Repositories
{
    public class CreateRepo
    {

        private readonly AppDbContext _context;
        public CreateRepo(AppDbContext context) {
            _context = context;
       }

        public async Task<bool> Create(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
               
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
