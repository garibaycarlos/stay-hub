namespace SignatureSuites.Api.Models.Dto.Login;

public class LoginResponseDto
{
    public string? Token { get; set; }
    public UserDto? UserDto { get; set; }
}
