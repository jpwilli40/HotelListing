using HotelListing.Controllers.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
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
    }
}
