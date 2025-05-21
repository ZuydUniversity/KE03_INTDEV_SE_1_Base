using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public static class MatrixIncDbInitializer
    {
        public static void Initialize(MatrixIncDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Customers.Any())
            {
                var customers = new Customer[]
                {
                    new Customer { Name = "Neo", Address = "123 Elm St" , Active=true},
                    new Customer { Name = "Morpheus", Address = "456 Oak St", Active = true },
                    new Customer { Name = "Trinity", Address = "789 Pine St", Active = true }
                };
                context.Customers.AddRange(customers);

                var orders = new Order[]
                {
                    new Order { Customer = customers[0], OrderDate = DateTime.Parse("2021-01-01")},
                    new Order { Customer = customers[0], OrderDate = DateTime.Parse("2021-02-01")},
                    new Order { Customer = customers[1], OrderDate = DateTime.Parse("2021-02-01")},
                    new Order { Customer = customers[2], OrderDate = DateTime.Parse("2021-03-01")}
                };
                context.Orders.AddRange(orders);
            }

            Category[] categories = Array.Empty<Category>();
            if (!context.Categories.Any())
            {
                categories = new Category[]
                {
                    new Category { Name = "Schepen" },
                    new Category { Name = "Stoelen" },
                    new Category { Name = "Wapens" }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges(); // Categories moeten eerst opgeslagen worden :/
            }
            else
            {
                categories = context.Categories.ToArray();
            }

            if (!context.Products.Any())
            {
                var products = new Product[]
                {
                    new Product
                    {
                        Name = "Nebuchadnezzar",
                        Description = "Het schip waarop Neo voor het eerst de echte wereld leert kennen",
                        Price = 10000.00m,
                        Category = categories.First(c => c.Name == "Schepen")
                    },
                    new Product
                    {
                        Name = "Jack-in Chair",
                        Description = "Stoel met een rugsteun en metalen armen waarin mensen zitten om ingeplugd te worden in de Matrix via een kabel in de nekpoort",
                        Price = 500.50m,
                        Category = categories.First(c => c.Name == "Stoelen")
                    },
                    new Product
                    {
                        Name = "EMP (Electro-Magnetic Pulse) Device",
                        Description = "Wapentuig op de schepen van Zion",
                        Price = 129.99m,
                        Category = categories.First(c => c.Name == "Wapens")
                    }
                };
                context.Products.AddRange(products);
            }

            if (!context.Parts.Any())
            {
                var parts = new Part[]
                {
                    new Part { Name = "Tandwiel", Description = "Overdracht van rotatie in bijvoorbeeld de motor of luikmechanismen"},
                    new Part { Name = "M5 Boutje", Description = "Bevestiging van panelen, buizen of interne modules"},
                    new Part { Name = "Hydraulische cilinder", Description = "Openen/sluiten van zware luchtsluizen of bewegende onderdelen"},
                    new Part { Name = "Koelvloeistofpomp", Description = "Koeling van de motor of elektronische systemen."}
                };
                context.Parts.AddRange(parts);
            }

            if (!context.FeaturedProducts.Any())
            {
                var featured = new FeaturedProduct[]
                {
                    new FeaturedProduct
                    {
                        Name = "Nebuchadnezzar",
                        ImagePath = "/images/products/nebuchadnezzar.jpg",
                        Price = 10000.00m,
                        Unit = "stuk"
                    },
                    new FeaturedProduct
                    {
                        Name = "Diepgroefkogellagers",
                        ImagePath = "/images/products/Diepgroefkogellagers-small.png",
                        Price = 2.99m,
                        Unit = "25 stuk"
                    }
                };
                context.FeaturedProducts.AddRange(featured);
            }

            context.SaveChanges();
        }
    }
}
