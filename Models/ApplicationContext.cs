using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace TEST_ASP_APP.Models
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated(); 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Store>(entity => { entity.Property(e => e.Name).IsRequired(); });
            modelBuilder.Entity<Store>().HasData
            (
                new Store
                {
                    StoreId = 1,
                    Name = "Burger King",
                    Adress = "г.Минск, ул. Козлова 19",
                    Opening_Time = new TimeSpan(7, 00, 00),
                    Closing_Time = new TimeSpan(22, 15, 00),
                    Products = new List<Product>()
                },
                new Store
                {
                    StoreId = 2,
                    Name = "KFC",
                    Adress = "г.Минск, ст. Немига",
                    Opening_Time = new TimeSpan(5, 00, 00),
                    Closing_Time = new TimeSpan(21, 30, 00),
                    Products = new List<Product>()
                }
            );
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.Store).WithMany(p => p.Products).HasForeignKey("StoreId");
            });
            modelBuilder.Entity<Product>().HasData
            (
                new Product { StoreId = 1, ProductId = 1, Name = "Bigmak", Description = "so big" },
                new Product { StoreId = 1, ProductId = 2, Name = "Pepsi", Description = "love" },
                new Product { StoreId = 2, ProductId = 3, Name = "Potato", Description = "Morgenshern" },
                new Product { StoreId = 2, ProductId = 4, Name = "Coca-cola", Description = "no love" }
            );
        }
    }
}
