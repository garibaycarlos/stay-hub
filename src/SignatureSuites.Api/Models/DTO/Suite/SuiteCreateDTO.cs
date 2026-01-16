using System.ComponentModel.DataAnnotations;

namespace SignatureSuites.Api.Models.DTO.Suite;

public class SuiteCreateDTO
{
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }
    public string? Details { get; set; }
    public double Rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string? ImageUrl { get; set; }
}
