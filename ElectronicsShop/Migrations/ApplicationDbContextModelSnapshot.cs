﻿// <auto-generated />
using ElectronicsShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ElectronicsShop.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ElectronicsShop.Models.Brand", b =>
                {
                    b.Property<int>("BrandID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CategoryID");

                    b.Property<string>("Name");

                    b.HasKey("BrandID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Brand");
                });

            modelBuilder.Entity("ElectronicsShop.Models.CartLine", b =>
                {
                    b.Property<int>("CartLineID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("OrderID");

                    b.Property<int?>("ProductID");

                    b.Property<int>("Quantity");

                    b.HasKey("CartLineID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductID");

                    b.ToTable("CartLine");
                });

            modelBuilder.Entity("ElectronicsShop.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ElectronicsShop.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<bool?>("Shipped");

                    b.HasKey("OrderID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ElectronicsShop.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Brand")
                        .IsRequired();

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<byte[]>("Image");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Price");

                    b.HasKey("ProductID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ElectronicsShop.Models.ProductStock", b =>
                {
                    b.Property<int>("ProductStockID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Booked");

                    b.Property<int>("InStock");

                    b.Property<int>("ProductIdent");

                    b.HasKey("ProductStockID");

                    b.ToTable("ProductStocks");
                });

            modelBuilder.Entity("ElectronicsShop.Models.SupplyProductParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ProductID");

                    b.Property<int>("SafetyRatio");

                    b.Property<int>("SupplyFrequency");

                    b.Property<int>("TimeToFormSupply");

                    b.HasKey("Id");

                    b.HasIndex("ProductID");

                    b.ToTable("SupplyProductParameters");
                });

            modelBuilder.Entity("ElectronicsShop.Models.Brand", b =>
                {
                    b.HasOne("ElectronicsShop.Models.Category")
                        .WithMany("Brands")
                        .HasForeignKey("CategoryID");
                });

            modelBuilder.Entity("ElectronicsShop.Models.CartLine", b =>
                {
                    b.HasOne("ElectronicsShop.Models.Order")
                        .WithMany("Lines")
                        .HasForeignKey("OrderID");

                    b.HasOne("ElectronicsShop.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");
                });

            modelBuilder.Entity("ElectronicsShop.Models.SupplyProductParameter", b =>
                {
                    b.HasOne("ElectronicsShop.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");
                });
#pragma warning restore 612, 618
        }
    }
}
