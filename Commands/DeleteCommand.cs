using Microsoft.EntityFrameworkCore;
using Practice.Data;
using MediatR;
namespace Practice.Commands
{
    public record DeleteCommand
        (
        int id
        ) :IRequest<DeleteResult>;

    public class DeleteResult
    { 
    public bool Success { get; set; }
    public string Message { get; set; }

    }

    public class DeleteHandler:IRequestHandler<DeleteCommand,DeleteResult>
    
    {
        private readonly AppDbContext _context;

        public DeleteHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteResult> Handle(DeleteCommand request,CancellationToken cancellationToken)
        {
            var product = await _context.Products
                 .FindAsync(new object[] { request.id }, cancellationToken);

            if (product == null)
                return new DeleteResult
                {
                    Success = false,
                    Message = "not found"
                };

            _context.Products.Remove(product);

            await _context.SaveChangesAsync(
                cancellationToken);

            return new DeleteResult
            { Success=true,
               Message="product succesfully deleted"
            };


        }
    }


}


