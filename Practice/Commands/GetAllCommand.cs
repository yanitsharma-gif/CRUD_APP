using MediatR;
using Practice.Models;
using Practice.Repositories;
namespace Practice.Commands
{
    public record GetAllCommand(
         
         ) : IRequest<GetAllResult>;

    public class GetAllResult {
        public List<Product> products {  get; set; }=new List<Product>();

    };

    public class GetAllResultHandler : IRequestHandler<GetAllCommand, GetAllResult> 
    {
        private readonly GetAllRepo _repo;
        public GetAllResultHandler(GetAllRepo repo)
        {
            _repo = repo;
        }

        public async Task<GetAllResult>Handle(GetAllCommand request,CancellationToken cancellation)
        {
            var result = await _repo.GetAll();

            return new GetAllResult
            {
                products = result
            };
        }
    }




}
