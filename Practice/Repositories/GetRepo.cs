using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Practice.Data;
using Practice.Models;
namespace Practice.Repositories
{
    public class GetRepo
    {

        private readonly AppDbContext _context;

        public GetRepo(AppDbContext context) {
            _context = context;
        
        }

        public async Task<Product?> GetById(int id)
        {

            try
            {
                var product = await _context.Products.FindAsync(id);

                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
