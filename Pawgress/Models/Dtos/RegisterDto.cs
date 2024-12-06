using System.ComponentModel.DataAnnotations;

public class RegisterDto // data transfer object
{
    [Required]
    [MinLength(3)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}