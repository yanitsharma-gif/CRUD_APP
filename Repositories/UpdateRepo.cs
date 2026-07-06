using MediatR;
using Practice.Data;
using Practice.Models;
using Practice.Responses;

namespace Practice.Repositories
{
    public class UpdateRepo
    {
        public readonly AppDbContext _context;

        public UpdateRepo(AppDbContext context)
        {
            _context = context;
        }
         public async Task<updateResponse> Update(Product updatedproduct,int id)
        {

            try
            {
                var product = await _context.Products
                        .FindAsync(id);
                product.Name = updatedproduct.Name;
                product.Price = updatedproduct.Price;
                await _context.SaveChangesAsync();
                return new updateResponse
                {
                    success = true,
                    product = product
                };
            }
            catch (Exception) {

               
                {
                    return new updateResponse
                    {
                        success = false,
                        product =null
                    };
                }
            }
        }
    }
}
