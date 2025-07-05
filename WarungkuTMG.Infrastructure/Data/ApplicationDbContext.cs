using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // DbSet properties for your entities go here
        public DbSet<Product> Products { get; set; }
        public DbSet<TransactionSale> TransactionSales { get; set; }
        public DbSet<TransactionSaleDetail> TransactionSaleDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Additional model configuration can go here
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Nasi Goreng",
                    Description = "Nasi goreng dengan ayam dan sayuran",
                    Price = 15000.00m,
                    ImageUrl = "https://placehold.co/600x400",
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                },
                new Product
                {
                    Id = 2,
                    Name = "Mie Goreng",
                    Description = "Mie goreng dengan telur dan sayuran",
                    Price = 12000.00m,
                    ImageUrl = "https://placehold.co/600x400",
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                }
            );

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("Users");
                b.Property(u => u.Id).HasMaxLength(128);
                b.Property(u => u.NormalizedUserName).HasMaxLength(128);
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Roles");
                b.Property(u => u.Id).HasMaxLength(128);
                b.Property(u => u.NormalizedName).HasMaxLength(128);
            });

            modelBuilder.Entity<ApplicationUserRole>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });

                b.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                b.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                b.ToTable("UserRoles");
            });
        }
    }
}
