using Microsoft.AspNetCore.Mvc;
using RoyalVilla_API.Models.DTO;
using RoyalVilla_API.Services;

namespace RoyalVilla_API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UserDTO>>> Register(RegistrationRequestDTO registrationRequestDTO)
    {
        if (registrationRequestDTO == null) return BadRequest(ApiResponse<object>.BadRequest("Registration data is required"));

        try
        {
            if (await _authService.EmailExistsAsync(registrationRequestDTO.Email))
            {
                return Conflict(ApiResponse<object>.Conflict($"User with email {registrationRequestDTO.Email} already exists"));
            }

            var user = await _authService.RegisterAsync(registrationRequestDTO);

            if (user is null) return BadRequest(ApiResponse<object>.BadRequest("Registration failed"));

            var response = ApiResponse<UserDTO>.CreatedAt(user, "User registered successfully");

            return CreatedAtAction(nameof(Register), response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred during registration", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login(LoginRequestDTO loginRequestDTO)
    {
        if (loginRequestDTO is null) return BadRequest(ApiResponse<object>.BadRequest("Login data is required"));
        
        try
        {
            var loginResponse = await _authService.LoginAsync(loginRequestDTO);

            if(loginResponse is null) return BadRequest(ApiResponse<object>.BadRequest("Login failed"));
            
            var response = ApiResponse<LoginResponseDTO>.Ok(loginResponse, "Login successful");

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred during login", ex.Message);
            
            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }
}
