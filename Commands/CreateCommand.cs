using Microsoft.EntityFrameworkCore;
using Practice.Models;
using MediatR;
using Practice.Data;
namespace Practice.Commands
{
    public record CreateCommand

        (
        Product product
        ) : IRequest<CreateResult>;

    public class CreateResult 
    
    { 
      public bool Success { get; set; }
        public string Message { get; set; }
    
    }

    public class CreateHandler : IRequestHandler<CreateCommand, CreateResult>
    {
        private readonly AppDbContext _context;

        public CreateHandler(AppDbContext context)
        {
            _context= context; ;
        }

        public async Task<CreateResult> Handle(CreateCommand request,CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(
                     request.product,
                     cancellationToken);
            try
            {
                await _context.SaveChangesAsync(
                    cancellationToken);
            }
            catch
            {
                return new CreateResult
                {
                    Success= false,
                    Message="not able to save"
                };
            }
            return new CreateResult 
            
            {
                Success =true,
                Message ="Data added successfully"
            };

        }

    }
   

    }
