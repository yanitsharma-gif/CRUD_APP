using Microsoft.EntityFrameworkCore;
using Practice.Models;
using MediatR;
using Practice.Data;
namespace Practice.Commands
{
    public record UpdateCommand
        (
        int id,
        Product updatedproduct
        ) : IRequest<UpdateResult>;
    public class UpdateResult
    { 
    public string Message { get; set; }
    public bool Success {  get; set; }
     public Product product { get; set; } = new Product();

    }

    public class UpdateResultHandler:IRequestHandler<UpdateCommand, UpdateResult>
    {
        private readonly AppDbContext _context;

        public UpdateResultHandler( AppDbContext context)
        {
            _context = context;
        }
      public async Task<UpdateResult> Handle(UpdateCommand request,CancellationToken cancellationToken)
        {
            try
            {
                var product = await _context.Products
                    .FindAsync(new object[] { request.id }, cancellationToken);
                if (product == null)
                    return new UpdateResult 
          
                    { Success =false,
                      Message="data not updated"
                    };


                product.Name = request.updatedproduct.Name;
                product.Price = request.updatedproduct.Price;

                await _context.SaveChangesAsync(
                    cancellationToken);

                return new UpdateResult
                {
                    Success = true,
                    Message = "data updated successfully",
                    product = product
                };
            }
            catch (Exception ex)
            {
                return new UpdateResult
                {
                    Success = false,
                    Message = ex.Message+" error during updation in database"
                };

            }
        }
    }



}
