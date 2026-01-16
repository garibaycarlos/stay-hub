using SignatureSuites.Api.Models.DTO.Login;

namespace SignatureSuites.Api.Services;

public interface IAuthService
{
    Task<UserDTO> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
    Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);
    Task<bool> EmailExistsAsync(string email);
}
