using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Repository.Context
{
    public class StockControlContext : DbContext
    {
        public StockControlContext(DbContextOptions<StockControlContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=LAPTOP-H9PBUUIN\\MSSQLSERVER2019;database=NRM1StokKontrol;Trusted_Connection=True;");
        }

        public DbSet<Category>? Categories { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderDetails>? OrderDetails { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Supplier>? Suppliers { get; set; }
        public DbSet<User>? Users { get; set; }
     
    }
}
