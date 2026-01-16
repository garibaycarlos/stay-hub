using System.ComponentModel.DataAnnotations;

namespace StayHub.Api.Models.DTO.Property;

public class PropertyUpdateDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }
    public string? Details { get; set; }
    public double Rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string? ImageUrl { get; set; }
}
