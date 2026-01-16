using System.ComponentModel.DataAnnotations;

namespace StayHub.Api.Models.DTO.Login;

public class LoginRequestDTO
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    public required string Password { get; set; }
}
