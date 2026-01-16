namespace SignatureSuites.Api.Models.DTO.Login;

public class LoginResponseDTO
{
    public string? Token { get; set; }
    public UserDTO? UserDTO { get; set; }
}
