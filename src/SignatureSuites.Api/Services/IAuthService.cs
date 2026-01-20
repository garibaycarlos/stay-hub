using SignatureSuites.Api.Models.Dto.Login;

namespace SignatureSuites.Api.Services;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegistrationRequestDto registrationRequestDTO);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDTO);
    Task<bool> EmailExistsAsync(string email);
}
