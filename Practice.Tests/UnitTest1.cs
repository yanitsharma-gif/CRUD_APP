namespace Practice.Tests;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Practice.Commands;
using Practice.Controllers;
using Practice.Data;
using Practice.DTO;
using Practice.Models;
using Practice.Repositories;
using Practice.Services;

public class UnitTest1
{
    [Fact]
    public  async Task Test1()
    {
        //Arrange
        var mediator = new Mock<IMediator>();
        
      mediator
     .Setup(x => x.Send(
         It.IsAny<RegisterUserCommand>(),
         It.IsAny<CancellationToken>()))
     .ReturnsAsync(new RegisterResult
     {
         Success = false,
         Message = "User not registered successfully"
     });
        //Act

        var controller = new AuthController(mediator.Object); 


        var request = new Register
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@gmail.com",
            Username = "john123",
            Password = "Password@123",
            Address = "Delhi"
        };

        //Assert

        var result = await controller.Register(request, CancellationToken.None);
      var okResult=  Assert.IsType<BadRequestObjectResult>(result);
      var Result= Assert.IsType<RegisterResult>(okResult.Value);

        Assert.Equal(400, okResult.StatusCode);
        Assert.Equal("User not registered successfully", Result.Message);
        Assert.False(Result.Success);
    }


    [Fact]
    public async Task Test2()
    {
        //Arrange
        var mediator = new Mock<IMediator>();

        mediator
       .Setup(x => x.Send(
           It.IsAny<RegisterUserCommand>(),
           It.IsAny<CancellationToken>()))
       .ReturnsAsync(new RegisterResult
       {
           Success = true,
           Message = "User  registered successfully"
       });
        //Act

        var controller = new AuthController(mediator.Object);


        var request = new Register
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@gmail.com",
            Username = "john123",
            Password = "Password@123",
            Address = "Delhi"
        };

        //Assert

        var result = await controller.Register(request, CancellationToken.None);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var Result = Assert.IsType<RegisterResult>(okResult.Value);

        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("User  registered successfully", Result.Message);
        Assert.True(Result.Success);
    }


    [Fact]

    public async Task Test3()
    {
        var mediator = new Mock<IMediator>();
       

       mediator.Setup(x => x.Send(It.IsAny<LoginUserCommand>(),It.IsAny<CancellationToken>()))
           .ReturnsAsync(new LoginResult
           {
               Success = true,
               Message = "User Login successfully",
               Token="tjmdcmx"
           }

            );
        var request = new Login
        {
            Username = "john123",
            Password = "Password@123",
           secretKey="12355"
        };

        var controller=new AuthController(mediator.Object);
        var result = await controller.Login(request, CancellationToken.None);

       var Okresult= Assert.IsType<OkObjectResult>(result);

        var obj = Assert.IsType<LoginResult>(Okresult.Value);
        Assert.Equal(200, Okresult.StatusCode);
        Assert.True(obj.Success);
        Assert.Equal("User Login successfully", obj.Message);
        Assert.Equal("tjmdcmx", obj.Token);

    }

    [Fact]

    public async Task Test4()
    {
        var mediator = new Mock<IMediator>();


        mediator.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LoginResult
            {
                Success = false,
                Message = "User can't Login successfully",
                Token = ""
            }

             );
        var request = new Login
        {
            Username = "john123",
            Password = "Password@123",
            secretKey = "12355"
        };

        var controller = new AuthController(mediator.Object);
        var result = await controller.Login(request, CancellationToken.None);

        var Okresult = Assert.IsType<BadRequestObjectResult>(result);

        var obj = Assert.IsType<LoginResult>(Okresult.Value);
        Assert.Equal(400, Okresult.StatusCode);
        Assert.False(obj.Success);
        Assert.Equal("User can't Login successfully", obj.Message);
        Assert.Equal("", obj.Token);

    }

}
