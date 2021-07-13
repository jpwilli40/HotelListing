﻿// <auto-generated />
using HotelListing.Controllers.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HotelListing.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210713181943_SeedingData")]
    partial class SeedingData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HotelListing.Controllers.Data.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries");

                    b.HasData(
                        new
                        {
                            CountryId = 1,
                            Name = "Jamaica",
                            ShortName = "JM"
                        },
                        new
                        {
                            CountryId = 2,
                            Name = "America",
                            ShortName = "US"
                        },
                        new
                        {
                            CountryId = 3,
                            Name = "England",
                            ShortName = "EN"
                        });
                });

            modelBuilder.Entity("HotelListing.Controllers.Data.Hotel", b =>
                {
                    b.Property<int>("HotelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.HasKey("HotelId");

                    b.HasIndex("CountryId");

                    b.ToTable("Hotels");

                    b.HasData(
                        new
                        {
                            HotelId = 1,
                            Address = "Negril",
                            CountryId = 1,
                            Name = "Sandals",
                            Rating = 4.5
                        },
                        new
                        {
                            HotelId = 2,
                            Address = "Los Angeles",
                            CountryId = 2,
                            Name = "Hilton",
                            Rating = 4.7999999999999998
                        },
                        new
                        {
                            HotelId = 3,
                            Address = "London",
                            CountryId = 3,
                            Name = "Pub City",
                            Rating = 4.2000000000000002
                        });
                });

            modelBuilder.Entity("HotelListing.Controllers.Data.Hotel", b =>
                {
                    b.HasOne("HotelListing.Controllers.Data.Country", "CountryKey")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CountryKey");
                });
#pragma warning restore 612, 618
        }
    }
}
