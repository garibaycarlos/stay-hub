using RoyalVilla_API.Models.DTO.Login;

namespace RoyalVilla_API.Services;

public interface IAuthService
{
    Task<UserDTO> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
    Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);
    Task<bool> EmailExistsAsync(string email);
}
