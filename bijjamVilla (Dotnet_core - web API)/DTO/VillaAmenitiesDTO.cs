using System.ComponentModel.DataAnnotations;

namespace bijjamVilla__Dotnet_core___web_API_.DTO
{
    public class VillaAmenitiesDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? Description { get; set; }


        [Required]
        public int VillaId { get; set; }

        public string? VillaName { get; set; }
    }
}
