using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bijjamVilla__Dotnet_core___web_API_.Model
{
    public class VillaAmenities
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        [Required]
        [ForeignKey(nameof(Villa))]
        public int VillaId { get; set; }

        public villa? Villa { get; set; }
    }
}
