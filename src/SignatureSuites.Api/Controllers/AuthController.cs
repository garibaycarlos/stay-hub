using Microsoft.AspNetCore.Mvc;
using SignatureSuites.Api.Models.Dto;
using SignatureSuites.Api.Models.Dto.Login;
using SignatureSuites.Api.Services;

namespace SignatureSuites.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegistrationRequestDto registrationRequestDto)
    {
        if (registrationRequestDto == null) return BadRequest(ApiResponse<object>.BadRequest("Registration data is required"));

        try
        {
            if (await _authService.EmailExistsAsync(registrationRequestDto.Email))
            {
                return Conflict(ApiResponse<object>.Conflict($"User with email {registrationRequestDto.Email} already exists"));
            }

            var user = await _authService.RegisterAsync(registrationRequestDto);

            if (user is null) return BadRequest(ApiResponse<object>.BadRequest("Registration failed"));

            var response = ApiResponse<UserDto>.CreatedAt(user, "User registered successfully");

            return CreatedAtAction(nameof(Register), response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred during registration", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(LoginRequestDto loginRequestDto)
    {
        if (loginRequestDto is null) return BadRequest(ApiResponse<object>.BadRequest("Login data is required"));

        try
        {
            var loginResponse = await _authService.LoginAsync(loginRequestDto);

            if (loginResponse is null) return BadRequest(ApiResponse<object>.BadRequest("Login failed"));

            var response = ApiResponse<LoginResponseDto>.Ok(loginResponse, "Login successful");

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred during login", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }
}
