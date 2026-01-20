using System.ComponentModel.DataAnnotations;

namespace SignatureSuites.Api.Models.Dto.Login;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    public required string Password { get; set; }
}
