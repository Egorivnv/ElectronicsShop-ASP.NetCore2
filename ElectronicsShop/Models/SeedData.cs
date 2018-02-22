using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ElectronicsShop.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                new Product
                {
                    Name = "Kayak",
                    Description = "A boat for one person",
                    Category = "Watersports",
                    Price = 275
                },
                new Product
                {
                    Name = "Lifejacket",
                    Description = "Protective and fashionable",
                    Category = "Watersports",
                    Price = 48.95m
                },
                new Product
                {
                    Name = "Soccer Ball",
                    Description = "FIFA-approved size and weight",
                    Category = "Soccer",
                    Price = 19.50m
                },
                new Product
                {
                    Name = "Corner Flags",
                    Description = "Give your playing field a professional touch",
                    Category = "Soccer",
                    Price = 34.95m
                },
                new Product
                {
                    Name = "Stadium",
                    Description = "Flat-packed 35,000-seat stadium",
                    Category = "Soccer",
                    Price = 79500
                },
                new Product
                {
                    Name = "Thinking Cap",
                    Description = "Improve brain efficiency by 75%",
                    Category = "Chess",
                    Price = 16
                },
                new Product
                {
                    Name = "Unsteady Chair",
                    Description = "Secretly give your opponent a disadvantage",
                    Category = "Chess",
                    Price = 29.95m
                },
                new Product
                {
                    Name = "Human Chess Board",
                    Description = "A fun game for the family",
                    Category = "Chess",
                    Price = 75
                },
                new Product
                {
                    Name = "Bling-Bling King",
                    Description = "Gold-plated, diamond-studded King",
                    Category = "Chess",
                    Price = 1200
                }
            );
                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Name = "Smartphones",
                        Brands = new List<Brand>
                        {
                            new Brand{Name="Sony"},
                            new Brand{Name="LG"},
                            new Brand{Name="Apple"},
                            new Brand{Name="Samsung"},
                            new Brand{Name="Nokia"},
                            new Brand{Name="Xiaomi"},
                            new Brand {Name="Huawei"}
                        }
                    },
                    new Category
                    {
                        Name = "Tablets",
                        Brands = new List<Brand>
                        {
                            new Brand{Name="Sony"},
                            new Brand{Name="Apple"},
                            new Brand{Name="Samsung"},
                            new Brand{Name="Xiaomi"},
                        }
                    },
                    new Category
                    {
                        Name = "Smart watches",
                        Brands = new List<Brand>
                        {
                            new Brand{Name="Apple"},
                            new Brand{Name="Samsung"},
                            new Brand{Name="Xiaomi"},
                        }
                    },
                    new Category
                    {
                        Name = "TV",
                        Brands = new List<Brand>
                        {
                            new Brand{Name="Sony"},
                            new Brand{Name="LG"},
                            new Brand{Name="Samsung"},
                            new Brand{Name="Panasonic"},
                            new Brand{Name="Philips"},
                        }
                    },
                    new Category
                    {
                        Name = "Audio technics",
                        Brands = new List<Brand>
                        {
                            new Brand{Name="Yamaha"},
                            new Brand{Name="JBL"},
                            new Brand{Name="DALI"},
                            new Brand{Name="KRK"},
                            new Brand{Name="Canton"},
                        }
                    },
                    new Category
                    {
                        Name = "Photo and video cameras",
                        Brands = new List<Brand>
                        {
                            new Brand{Name="Canon"},
                            new Brand{Name="Nikon"},
                            new Brand{Name="Fulifilm"},
                            new Brand{Name="Olympus"},
                            new Brand{Name="Sony"},
                            new Brand{Name="Samsung"},
                        }
                    }
                    );
                context.SaveChanges();
            }



        }
    }
}