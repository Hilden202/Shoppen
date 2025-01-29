using System;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Models
{
	public class MyDbContext : DbContext
	{
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<SelectedProduct> SelectedProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:patrikdb.database.windows.net,1433;Initial Catalog=patrikdb;"
                + "Persist Security Info=False;User ID=patrik;Password=!Q2w3e4r;MultipleActiveResultSets=False;Encrypt=True;"
                + "TrustServerCertificate=False;Connection Timeout=30;");
        }

    }
}