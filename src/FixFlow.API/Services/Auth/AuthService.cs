using FixFlow.API.Data;
using FixFlow.API.Interfaces.Auth;
using FixFlow.Shared.Dtos.Auth.Login;
using FixFlow.Shared.Dtos.Auth.Register;
using FixFlow.Shared.Dtos.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FixFlow.API.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .IgnoreQueryFilters()
                .Include(u => u.Tenant)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null; // credenciales invalidas

            if (user.Tenant == null || !user.Tenant.IsActive)
                throw new UnauthorizedAccessException("El espacio de trabajo de la empresa esta inactivo.");

            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiryMinutes"]!)),
                TenantId = user.TenantId.ToString(),
                User = new UserDto
                {
                    Id = user.Id.ToString(),
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role!
                }
            };
        }

        public async Task<bool> RegisterTenantAsync(RegisterTenantRequestDto request)
        {
            var emailExists = await _context.Users.IgnoreQueryFilters().AnyAsync(u => u.Email == request.Email);
            if (emailExists)
                throw new InvalidOperationException("El correo electrónico ya está registrado.");

            var newTenant = new Entities.Tenant
            {
                Name = request.CompanyName,
                TaxId = request.CompanyRut,
                Industry = request.Services ?? "",
                IsActive = true,
                Plan = "Free",
                CreatedAt = DateTime.UtcNow
            };

            var adminUser = new Entities.User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Admin",
                TaxId = request.UserRut
            };

            newTenant.Users.Add(adminUser);

            _context.Tenants.Add(newTenant);

            await _context.SaveChangesAsync();

            return true;
        }

        private string GenerateJwtToken(Entities.User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("tenantId", user.TenantId.ToString()),
                new Claim(ClaimTypes.Role, user.Role!)
            };
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!)),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
