using System.ComponentModel.DataAnnotations;

namespace SignatureSuites.Api.Models;

public class Suite
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }
    public string? Details { get; set; }
    public decimal Rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public ICollection<SuiteAmenity> SuiteAmenities { get; set; } = new List<SuiteAmenity>();
}
