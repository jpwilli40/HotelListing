using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers.Data
{
    public class DatabaseContext : DbContext  //Bridge between application and DB
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Country> Countries { get; set; }  //Country is set of what, Countries is name of set
        public DbSet<Hotel> Hotels { get; set; }

        #region Data Seeding
        protected override void OnModelCreating(ModelBuilder builder)  //used to preload DB with test data
        {
            builder.Entity<Country>().HasData(
                new Country
                {
                    CountryId = 1,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    CountryId = 2,
                    Name = "America",
                    ShortName = "US"
                },
                new Country
                {
                    CountryId = 3,
                    Name = "England",
                    ShortName = "EN"
                }
                );

            builder.Entity<Hotel>().HasData(
                new Hotel
                {
                    HotelId = 1,
                    Name = "Sandals",
                    Address = "Negril",
                    CountryId = 1,
                    Rating = 4.5
                },
                new Hotel
                {
                    HotelId = 2,
                    Name = "Hilton",
                    Address = "Los Angeles",
                    CountryId = 2,
                    Rating = 4.8
                },
                new Hotel
                {
                    HotelId = 3,
                    Name = "Pub City",
                    Address = "London",
                    CountryId = 3,
                    Rating = 4.2
                }
                );
        } 
        #endregion
    }
}
