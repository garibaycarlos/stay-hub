using System.ComponentModel.DataAnnotations;

namespace SignatureSuites.Api.Models.Dto.Amenity;

public class AmenityCreateDto
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    public string? Description { get; set; }
}
