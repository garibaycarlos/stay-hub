using System.ComponentModel.DataAnnotations;

namespace RoyalVilla_API.Models.DTO.Login;

public class LoginRequestDTO
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    public required string Password { get; set; }
}
