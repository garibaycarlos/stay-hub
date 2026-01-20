
using SignatureSuites.Api.Models.Dto.Amenity;

namespace SignatureSuites.Api.Models.Dto.Suite;

public class SuiteDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Details { get; set; }
    public double Rate { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    public string? ImageUrl { get; set; }
    public IEnumerable<AmenityDto> Amenities { get; set; } = [];
}
