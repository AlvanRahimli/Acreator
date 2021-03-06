﻿using Acreator.Models;
using Microsoft.EntityFrameworkCore;

namespace Acreator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}