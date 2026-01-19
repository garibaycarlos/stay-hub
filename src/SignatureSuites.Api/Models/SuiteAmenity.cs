namespace SignatureSuites.Api.Models;

public class SuiteAmenity
{
    public int SuiteId { get; set; }
    public Suite Suite { get; set; } = null!;

    public int AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
