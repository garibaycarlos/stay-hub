using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTO.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoyalVilla_API.Services;

public class AuthService(ApplicationDbContext db, IMapper mapper, IConfiguration configuration) : IAuthService
{
    private readonly ApplicationDbContext _db = db;
    private readonly IMapper _mapper = mapper;
    private readonly IConfiguration _configuration = configuration;

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
    {
        try
        {
            if (loginRequestDTO is null) throw new Exception("Login request cannot be null");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());

            if (user is null || user.Password != loginRequestDTO.Password) return null;

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                UserDTO = _mapper.Map<UserDTO>(user),
                Token = token
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred during user login", ex);
        }
    }

    public async Task<UserDTO> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
    {
        try
        {
            if (await EmailExistsAsync(registrationRequestDTO.Email))
                throw new InvalidOperationException($"User with email '{registrationRequestDTO.Email}' already exists");

            var user = new User
            {
                Email = registrationRequestDTO.Email,
                Name = registrationRequestDTO.Name,
                Password = registrationRequestDTO.Password,
                Role = registrationRequestDTO.Role ?? "Customer",
                CreatedDate = DateTime.UtcNow
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred during user registration", ex);
        }
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSettings").GetValue<string>("Secret")!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
