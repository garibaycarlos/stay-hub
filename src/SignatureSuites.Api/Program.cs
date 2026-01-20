using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using SignatureSuites.Api.Data;
using SignatureSuites.Api.Models;
using SignatureSuites.Api.Models.Dto.Amenity;
using SignatureSuites.Api.Models.Dto.Login;
using SignatureSuites.Api.Models.Dto.Suite;
using SignatureSuites.Api.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtSettings").GetValue<string>("Secret")!);

// Add services to the container.
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter JWT Bearer token"
            }
        };

        document.Security =
        [
            new OpenApiSecurityRequirement
            {
                { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
            }
        ];

        return Task.CompletedTask;
    });
});

builder.Services.AddAutoMapper(o =>
{
    // Suite
    o.CreateMap<Suite, SuiteDto>()
        .ForMember(dest => dest.Amenities,
            opt => opt.MapFrom(src =>
                src.SuiteAmenities.Select(sa => sa.Amenity)));

    o.CreateMap<Suite, SuiteCreateDto>().ReverseMap();
    o.CreateMap<Suite, SuiteUpdateDto>().ReverseMap();
    o.CreateMap<SuiteUpdateDto, SuiteDto>().ReverseMap();

    // Amenity
    o.CreateMap<Amenity, AmenityDto>().ReverseMap();
    o.CreateMap<Amenity, AmenityCreateDto>().ReverseMap();
    o.CreateMap<Amenity, AmenityUpdateDto>().ReverseMap();

    // Login
    o.CreateMap<User, UserDto>().ReverseMap();
});

builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

await SeedDataAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();
}
