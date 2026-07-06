using Microsoft.EntityFrameworkCore;
using Practice.Data;
using MediatR;
using Practice.Repositories;
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
        private readonly DeleteRepo _repo;

        public DeleteHandler(DeleteRepo repo)
        {
            _repo = repo;
        }

        public async Task<DeleteResult> Handle(DeleteCommand request,CancellationToken cancellationToken)
        {


            var success = await _repo.Delete(request.id);
            if (!success)
                return new DeleteResult
                {
                    Success = false,
                    Message = "not found"
                };

            return new DeleteResult
            { Success=true,
               Message="product succesfully deleted"
            };


        }
    }


}


