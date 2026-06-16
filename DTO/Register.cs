using System.ComponentModel.DataAnnotations;

namespace Practice.DTO;

public class Register
{


    public string id { get; set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2-50 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters allowed")]
    public string FirstName { get; set; }= string.Empty;


    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; }=string.Empty;



    [Required]
    [StringLength(30, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, underscore")]
    public string Username { get; set; } = string.Empty;


    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; }=string.Empty ;
    public string Address { get; set;  } = string.Empty;
    
}