using MediatR;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models;

namespace Practice.Commands
{
    public record GetCommand(
        int Id
        ) : IRequest<GetResult>;

        public class GetResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Product list { get; set; } = new Product();
    }


    public class GetResultHandler : IRequestHandler<GetCommand, GetResult>
    {

        private readonly AppDbContext _context;
        public GetResultHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetResult> Handle(GetCommand request,
        CancellationToken cancellationToken)
        {
            var product = await _context.Products
                     .FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
                return new GetResult
                {
                    Success = false,
                    Message = "data is not found",


                };
                

            return new GetResult
           {
            Success=true,
            Message="data found",
            list=product
            };


            
        }
    
    }




}
