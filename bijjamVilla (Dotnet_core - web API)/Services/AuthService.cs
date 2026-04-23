using AutoMapper;

using Microsoft.EntityFrameworkCore;

using bijjamVilla__Dotnet_core___web_API_.Data;
using bijjamVilla__Dotnet_core___web_API_.DTO;
using bijjamVilla__Dotnet_core___web_API_.Model;

using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace bijjamVilla__Dotnet_core___web_API_.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext db, IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return await _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());
                if (user == null || user.Password != loginRequestDTO.Password)
                {
                    return null;
                }

                // generate JWT token here and return it along with user details
                var token = GenerateJwtToken(user);
                return new LoginResponseDTO
                {
                    UserDTO = _mapper.Map<UserDTO>(user),
                    Token = token
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while registering the user. Please try again later.{ex.Message}");
            }
        }


        public async Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                if (await IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    throw new InvalidOperationException($"User with email '{registerationRequestDTO.Email}' already exists.");
                }

                User user = new()
                {
                    Email = registerationRequestDTO.Email,
                    Name = registerationRequestDTO.Name,
                    Password = registerationRequestDTO.Password,
                    Role = string.IsNullOrEmpty(registerationRequestDTO?.Role) ? "customer" : registerationRequestDTO.Role,
                    CreatedDate = DateTime.UtcNow
                };

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while registering the user. Please try again later. {ex.Message}");
            }
        }
        public string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("jwtSettings")["SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
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
}
