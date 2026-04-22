using bijjamVilla__Dotnet_core___web_API_.DTO;

namespace bijjamVilla__Dotnet_core___web_API_.Services
{
    public interface IAuthService
    {
        Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO);
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<bool> IsEmailExistsAsync(string email);
    }
}
