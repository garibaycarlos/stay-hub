using System.ComponentModel.DataAnnotations;

namespace SignatureSuites.Api.Models;

public class Amenity
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public ICollection<SuiteAmenity> SuiteAmenities { get; set; } = new List<SuiteAmenity>();
}
