using Microsoft.EntityFrameworkCore;
using SignatureSuites.Api.Models;

namespace SignatureSuites.Api.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Suite> Suite { get; set; }
    public DbSet<Amenity> Amenity { get; set; }
    public DbSet<SuiteAmenity> SuiteAmenities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SuiteAmenity>()
    .HasKey(pa => new { pa.SuiteId, pa.AmenityId });

        modelBuilder.Entity<SuiteAmenity>()
            .HasOne(pa => pa.Suite)
            .WithMany(p => p.SuiteAmenities)
            .HasForeignKey(pa => pa.SuiteId);

        modelBuilder.Entity<SuiteAmenity>()
            .HasOne(pa => pa.Amenity)
            .WithMany(a => a.SuiteAmenities)
            .HasForeignKey(pa => pa.AmenityId);

        modelBuilder.Entity<Suite>().HasData(
            new Suite
            {
                Id = 1,
                Name = "Royal Villa",
                Details = "Luxurious villa with stunning ocean views and private beach access.",
                Rate = 500.0,
                Sqft = 2500,
                Occupancy = 6,
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg",
                CreatedDate = new DateTime(2024, 1, 1),
                UpdatedDate = new DateTime(2024, 1, 1)
            },
            new Suite
            {
                Id = 2,
                Name = "Diamond Villa",
                Details = "Elegant villa with marble interiors and panoramic mountain views.",
                Rate = 750.0,
                Sqft = 3200,
                Occupancy = 8,
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg",
                CreatedDate = new DateTime(2024, 1, 15),
                UpdatedDate = new DateTime(2024, 1, 15)
            },
            new Suite
            {
                Id = 3,
                Name = "Pool Villa",
                Details = "Modern villa featuring an infinity pool and outdoor entertainment area.",
                Rate = 350.0,
                Sqft = 1800,
                Occupancy = 4,
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg",
                CreatedDate = new DateTime(2024, 2, 1),
                UpdatedDate = new DateTime(2024, 2, 1)
            },
            new Suite
            {
                Id = 4,
                Name = "Luxury Villa",
                Details = "Premium villa with spa facilities and concierge services.",
                Rate = 900.0,
                Sqft = 4000,
                Occupancy = 10,
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg",
                CreatedDate = new DateTime(2024, 2, 14),
                UpdatedDate = new DateTime(2024, 2, 14)
            },
            new Suite
            {
                Id = 5,
                Name = "Garden Villa",
                Details = "Charming villa surrounded by tropical gardens and nature trails.",
                Rate = 275.0,
                Sqft = 1500,
                Occupancy = 3,
                ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg",
                CreatedDate = new DateTime(2024, 3, 1),
                UpdatedDate = new DateTime(2024, 3, 1)
            });
    }
}
