using System.ComponentModel.DataAnnotations;

namespace SignatureSuites.Api.Models.Dto.Amenity;

public class AmenityUpdateDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    public string? Description { get; set; }
}
