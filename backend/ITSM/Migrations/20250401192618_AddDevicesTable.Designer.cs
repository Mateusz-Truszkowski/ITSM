﻿// <auto-generated />
using System;
using ITSM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ITSM.Migrations
{
    [DbContext(typeof(ITSMContext))]
    [Migration("20250401192618_AddDevicesTable")]
    partial class AddDevicesTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ITSM.Entity.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AcquisitionDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepreciationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Devices");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AcquisitionDate = new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepreciationDate = new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Laptop ultrabook, 13 cali, i7, 16GB RAM",
                            Name = "Laptop Dell XPS 13",
                            Status = "Active",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            AcquisitionDate = new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepreciationDate = new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Smartphone z ekranem 6.2 cala, 8GB RAM",
                            Name = "Smartphone Samsung Galaxy S21",
                            Status = "Active",
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            AcquisitionDate = new DateTime(2022, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepreciationDate = new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Monitor 27 cali, 4K",
                            Name = "Monitor LG 27",
                            Status = "Active",
                            UserId = 1
                        },
                        new
                        {
                            Id = 4,
                            AcquisitionDate = new DateTime(2021, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepreciationDate = new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Drukarka laserowa, czarno-biała",
                            Name = "Printer HP LaserJet Pro",
                            Status = "Inactive",
                            UserId = 2
                        },
                        new
                        {
                            Id = 5,
                            AcquisitionDate = new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepreciationDate = new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Tablet 12.9 cala, 256GB, iOS",
                            Name = "Tablet iPad Pro 12.9",
                            Status = "Active",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("ITSM.Entity.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ContractingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SLA")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Services");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContractingDate = new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Providing reliable and fast web hosting services.",
                            Name = "Web Hosting",
                            SLA = 99,
                            Status = "Active"
                        },
                        new
                        {
                            Id = 2,
                            ContractingDate = new DateTime(2022, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Scalable cloud storage solutions for businesses of all sizes.",
                            Name = "Cloud Storage",
                            SLA = 99,
                            Status = "Active"
                        },
                        new
                        {
                            Id = 3,
                            ContractingDate = new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Email marketing campaigns, automation, and reporting.",
                            Name = "Email Marketing",
                            SLA = 98,
                            Status = "Inactive"
                        },
                        new
                        {
                            Id = 4,
                            ContractingDate = new DateTime(2022, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Secure backup and disaster recovery solutions.",
                            Name = "Data Backup",
                            SLA = 97,
                            Status = "Active"
                        },
                        new
                        {
                            Id = 5,
                            ContractingDate = new DateTime(2023, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Search engine optimization to increase website visibility.",
                            Name = "SEO Optimization",
                            SLA = 95,
                            Status = "Active"
                        });
                });

            modelBuilder.Entity("ITSM.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Occupation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreationDate = new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jdoe@example.com",
                            Group = "Admin",
                            Login = "jdoe",
                            Name = "John",
                            Occupation = "Software Engineer",
                            Password = "Password123",
                            Status = "Active",
                            Surname = "Doe"
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2022, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "asmith@example.com",
                            Group = "User",
                            Login = "asmith",
                            Name = "Alice",
                            Occupation = "Product Manager",
                            Password = "SecurePass456",
                            Status = "Active",
                            Surname = "Smith"
                        },
                        new
                        {
                            Id = 3,
                            CreationDate = new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "bjohnson@example.com",
                            Group = "Moderator",
                            Login = "bjohnson",
                            Name = "Bob",
                            Occupation = "QA Engineer",
                            Password = "TestPass789",
                            Status = "Inactive",
                            Surname = "Johnson"
                        },
                        new
                        {
                            Id = 4,
                            CreationDate = new DateTime(2022, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "klane@example.com",
                            Group = "User",
                            Login = "klane",
                            Name = "Kate",
                            Occupation = "UX Designer",
                            Password = "KatePass999",
                            Status = "Active",
                            Surname = "Lane"
                        },
                        new
                        {
                            Id = 5,
                            CreationDate = new DateTime(2023, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mwhite@example.com",
                            Group = "Admin",
                            Login = "mwhite",
                            Name = "Michael",
                            Occupation = "CTO",
                            Password = "MikePass111",
                            Status = "Suspended",
                            Surname = "White"
                        });
                });

            modelBuilder.Entity("ITSM.Entity.Device", b =>
                {
                    b.HasOne("ITSM.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
