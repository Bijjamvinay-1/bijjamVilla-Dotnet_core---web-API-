using System.ComponentModel.DataAnnotations;

namespace bijjamVilla__Dotnet_core___web_API_.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Email { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Role { get; set; } = default!;
    }
}