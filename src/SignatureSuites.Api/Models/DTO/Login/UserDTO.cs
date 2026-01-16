using System.ComponentModel.DataAnnotations;

namespace SignatureSuites.Api.Models.DTO.Login;

public class UserDTO
{
    public string? Email { get; set; }

    public string? Name { get; set; }

    public string? Role { get; set; }
}
