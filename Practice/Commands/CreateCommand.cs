using Microsoft.EntityFrameworkCore;
using Practice.Models;
using MediatR;
using Practice.Data;
using Practice.Repositories;
using Microsoft.AspNetCore.Components.Forms.Mapping;
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
        private readonly CreateRepo _repo;

        public CreateHandler(CreateRepo repo)
        {
            _repo = repo ;
        }

        public async Task<CreateResult> Handle(CreateCommand request,CancellationToken cancellationToken)
        {



            bool val = await _repo.Create(request.product);
            if (!val)
            {
                new CreateResult { 
                    Message ="Data not added succesfully",
                    Success=false
          
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
