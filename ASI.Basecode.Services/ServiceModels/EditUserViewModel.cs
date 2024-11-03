using System.ComponentModel.DataAnnotations;

public class EditUserViewModel
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    public string Fname { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    public string Lname { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    // Optional password fields
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    public string Role { get; set; }

    public bool IsActive { get; set; }
} 