using MediatR;
using Practice.Data;
using Practice.Models;

namespace Practice.Repositories
{
    public class DeleteRepo
    {
        private readonly AppDbContext _context;

        public DeleteRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var product = await _context.Products
                    .FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception)
            {
                return false;
            }
           
            
            return true;

        }

    }
}
