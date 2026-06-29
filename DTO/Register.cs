using System.ComponentModel.DataAnnotations;

namespace Practice.DTO;

public class Register
{


    public string id { get; set; } = Guid.NewGuid().ToString();

    public string Email { get; set; } = string.Empty;

    
    public string FirstName { get; set; }= string.Empty;


    
    public string LastName { get; set; }=string.Empty;



   
    public string Username { get; set; } = string.Empty;



    public string Password { get; set; }=string.Empty ;
    public string Address { get; set;  } = string.Empty;
    
    
}