using Microsoft.EntityFrameworkCore;
using Practice.Models;
using MediatR;
using Practice.Data;
using Practice.Repositories;
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
        private readonly UpdateRepo _repo;

        public UpdateResultHandler( UpdateRepo repo)
        {
            _repo = repo;
        }
      public async Task<UpdateResult> Handle(UpdateCommand request,CancellationToken cancellationToken)
        {

            var obj = await _repo.Update(request.updatedproduct, request.id);
                if (obj.success == false)
                    return new UpdateResult 
          
                    { Success =false,
                      Message="data not updated"
                    };


               

                return new UpdateResult
                {
                    Success = true,
                    Message = "data updated successfully",
                    product = obj.product
                };
            }
           

            
        }
    }



