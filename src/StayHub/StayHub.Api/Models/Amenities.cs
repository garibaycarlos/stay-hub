using System.ComponentModel.DataAnnotations;

namespace StayHub.Api.Models;

public class Amenities
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
