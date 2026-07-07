using MediatR;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models;
using Practice.Repositories;

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

        private readonly GetRepo _repo;
        public GetResultHandler(GetRepo repo)
        {
            _repo = repo;
        }

        public async Task<GetResult> Handle(GetCommand request,
        CancellationToken cancellationToken)
        {
            var product = await _repo.GetById(request.Id);
                    

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
