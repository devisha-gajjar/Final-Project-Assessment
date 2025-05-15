using System.ComponentModel.DataAnnotations;

namespace WebApp.Entities.ViewModel;

public class RegisterViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    [RegularExpression(@"^[A-Za-z]+(?:\s[A-Za-z]+)*$", ErrorMessage = "Name can only contain alphabets.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(30, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 30 characters")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
       ErrorMessage = "Please enter a properly formatted email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least 8 characters, including uppercase, lowercase, digit, and special character.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Please enter Confirm Password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPass { get; set; } = null!;

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Phone number is not Valid")]
    public string Phone { get; set; }
}
