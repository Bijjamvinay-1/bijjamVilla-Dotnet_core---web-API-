using System.ComponentModel.DataAnnotations;

namespace BijjamVillaDTO
{
    public class villaUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public required string Name { get; set; }
        public string? Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }

    }
}
