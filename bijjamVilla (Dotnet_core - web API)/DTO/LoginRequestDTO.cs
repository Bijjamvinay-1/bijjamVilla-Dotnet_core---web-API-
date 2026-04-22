using System.ComponentModel.DataAnnotations;

namespace bijjamVilla__Dotnet_core___web_API_.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
