using Microsoft.EntityFrameworkCore;
using SignatureSuites.Api.Models;

namespace SignatureSuites.Api.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Suite> Suites { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<SuiteAmenity> SuiteAmenities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Suite>()
            .Property(s => s.Rate)
            .HasPrecision(10, 2);

        modelBuilder.Entity<SuiteAmenity>()
            .HasKey(sa => new { sa.SuiteId, sa.AmenityId });

        modelBuilder.Entity<SuiteAmenity>()
            .HasOne(sa => sa.Suite)
            .WithMany(s => s.SuiteAmenities)
            .HasForeignKey(sa => sa.SuiteId);

        modelBuilder.Entity<SuiteAmenity>()
            .HasOne(sa => sa.Amenity)
            .WithMany(a => a.SuiteAmenities)
            .HasForeignKey(sa => sa.AmenityId);

        var seedDate = new DateTime(2024, 1, 1);

        // Suite seed data
        modelBuilder.Entity<Suite>().HasData(
        new Suite
        {
            Id = 1,
            Name = "Royal Ocean Suite",
            Details = "Luxury suite with private pool and ocean views.",
            Rate = 600,
            Sqft = 2800,
            Occupancy = 6,
            ImageUrl = "https://example.com/suite1.jpg",
            CreatedDate = seedDate
        },
        new Suite
        {
            Id = 2,
            Name = "Diamond Mountain Suite",
            Details = "Elegant suite overlooking the mountains.",
            Rate = 750,
            Sqft = 3200,
            Occupancy = 8,
            ImageUrl = "https://example.com/suite2.jpg",
            CreatedDate = seedDate
        }
    );

        // Amenity seed data
        modelBuilder.Entity<Amenity>().HasData(
            new Amenity
            {
                Id = 1,
                Name = "Wi-Fi",
                Description = "High-speed wireless internet",
                CreatedDate = seedDate
            },
            new Amenity
            {
                Id = 2,
                Name = "Air Conditioning",
                Description = "Climate-controlled rooms",
                CreatedDate = seedDate
            },
            new Amenity
            {
                Id = 3,
                Name = "Private Pool",
                Description = "Exclusive private swimming pool",
                CreatedDate = seedDate
            },
            new Amenity
            {
                Id = 4,
                Name = "Ocean View",
                Description = "Unobstructed ocean views",
                CreatedDate = seedDate
            },
            new Amenity
            {
                Id = 5,
                Name = "Room Service",
                Description = "24/7 in-room dining",
                CreatedDate = seedDate
            }
    );

        // SuiteAmenity join seed data
        modelBuilder.Entity<SuiteAmenity>().HasData(
            new SuiteAmenity { SuiteId = 1, AmenityId = 1, CreatedDate = seedDate },
            new SuiteAmenity { SuiteId = 1, AmenityId = 3, CreatedDate = seedDate },
            new SuiteAmenity { SuiteId = 1, AmenityId = 4, CreatedDate = seedDate },

            new SuiteAmenity { SuiteId = 2, AmenityId = 1, CreatedDate = seedDate },
            new SuiteAmenity { SuiteId = 2, AmenityId = 2, CreatedDate = seedDate },
            new SuiteAmenity { SuiteId = 2, AmenityId = 5, CreatedDate = seedDate }
        );
    }
}
