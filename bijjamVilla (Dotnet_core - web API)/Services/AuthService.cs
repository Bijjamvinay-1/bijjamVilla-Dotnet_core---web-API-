using AutoMapper;
using Microsoft.EntityFrameworkCore;

using bijjamVilla__Dotnet_core___web_API_.Data;
using bijjamVilla__Dotnet_core___web_API_.DTO;
using bijjamVilla__Dotnet_core___web_API_.Model;

namespace bijjamVilla__Dotnet_core___web_API_.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AuthService(ApplicationDbContext db, IConfiguration configuration, IMapper mapper) 
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return await _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
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
                throw new InvalidOperationException("An error occurred while registering the user. Please try again later.", ex);
            }
        }
    }
}
