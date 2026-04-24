using System.ComponentModel.DataAnnotations;

namespace BijjamVillaDTO
{
    public class villaCreateDTO    // Data Transfer Object
    {
        [MaxLength (50)]
        [Required]
        public required string Name { get; set; }
        public string? Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
    }
}
