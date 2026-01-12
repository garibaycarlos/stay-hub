using RoyalVilla_API.Models.DTO;

namespace RoyalVilla_API.Services;

public interface IAuthService
{
    Task<UserDTO> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
    Task<LoginRequestDTO> LoginAsync(LoginRequestDTO loginRequestDTO);
    Task<bool> EmailExistsAsync(string email);
}
