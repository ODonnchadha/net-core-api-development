using HotelListing.Configurations;
using HotelListing.Entities;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HotelListing.Contexts
{
    public class HotelListingContext : IdentityDbContext<User>
    {
        public HotelListingContext([NotNullAttribute] DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new IdentityRoleConfiguration());

            builder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "Jamaica", ShortName = "JM" },
                new Country { Id = 2, Name = "Bahamas", ShortName = "BS" },
                new Country { Id = 3, Name = "Cayman Island", ShortName = "CI" }
            );
            builder.Entity<Hotel>().HasData(
                new Hotel { Id = 1, Name = "Sandals Resort & Spa", Address = "Negril", Rating = 5.0, CountryId = 1 },
                new Hotel { Id = 2, Name = "Grand Pallidium", Address = "Nassua", Rating = 4.0, CountryId = 2 },
                new Hotel { Id = 3, Name = "Comfort Suites", Address = "Gerogetown", Rating = 1.3, CountryId = 3 }
            );
        }
        public DbSet<Country> Countries { get; set; }
        DbSet<Hotel> Hotels { get; set; }
    }
}
