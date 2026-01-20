using System.ComponentModel.DataAnnotations;

namespace SignatureSuites.Api.Models.Dto.Suite;

public class SuiteUpdateDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }
    public string? Details { get; set; }
    public decimal Rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string? ImageUrl { get; set; }
}
