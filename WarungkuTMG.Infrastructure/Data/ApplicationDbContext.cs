using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WarungkuTMG.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WarungkuTMG.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // DbSet properties for your entities go here
        public DbSet<Product> Products { get; set; }
        public DbSet<TransactionSale> TransactionSales { get; set; }
        public DbSet<TransactionSaleDetail> TransactionSaleDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Additional model configuration can go here
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Nasi Goreng",
                    Description = "Nasi goreng dengan ayam dan sayuran",
                    Price = 15000,
                    ImageUrl = "/images/ProductImages/21fc1854-affb-466a-ad78-07a87a47c8df.jpeg",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 2,
                    Name = "Mie Goreng",
                    Description = "Mie goreng dengan telur dan sayuran",
                    Price = 10000,
                    ImageUrl = "/images/ProductImages/f52d4323-b225-46db-9b28-423b347264aa.jpeg",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 3,
                    Name = "Mie Ayam",
                    Description = "Mie Ayam dengan ceker",
                    Price = 15000,
                    ImageUrl = "/images/ProductImages/f0d826df-896a-4dfe-ab0a-77b8dbf2c98e.jpeg",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 4,
                    Name = "Pisang Goreng",
                    Description = "Pisang ambon terbaik",
                    Price = 5000,
                    ImageUrl = "/images/ProductImages/a7d7aac3-e107-4d99-872f-5fa953615873.jpeg",
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now
                }
            );

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("Users");
                b.Property(u => u.Id).HasMaxLength(128);
                b.Property(u => u.NormalizedUserName).HasMaxLength(128);
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaims");
                b.Property(u => u.Id).HasMaxLength(128);
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UserTokens");
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Roles");
                b.Property(u => u.Id).HasMaxLength(128);
                b.Property(u => u.NormalizedName).HasMaxLength(128);
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaims");
                b.Property(u => u.Id).HasMaxLength(128);
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

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }



        private void AddTimestamps()
        {
            var entities = ChangeTracker
                .Entries()
                .Where(x => x.Entity is BaseEntity || x.Entity is ApplicationUser
                && (x.State == EntityState.Added
                || x.State == EntityState.Modified));
            if (entities.FirstOrDefault() == null)
            {
                return;
            }
            try
            {
                var userName = "";
                if (_httpContextAccessor.HttpContext == null)
                {
                    return;
                }
                if (!_httpContextAccessor.HttpContext.User.HasClaim(c => c.Type == ClaimTypes.Name))
                {
                    userName = "Anonymous";
                }
                else
                {
                    userName = _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
                }

                var currentUsername = !string.IsNullOrEmpty(userName)
                    ? userName
                    : "Anonymous";

                if (entities.FirstOrDefault().Entity is BaseEntity)
                {
                    foreach (var entity in entities)
                    {
                        if (entity.State == EntityState.Added)
                        {
                            ((BaseEntity)entity.Entity).CreatedDate = DateTime.Now;
                            if (currentUsername != "Anonymous")
                            {
                                ((BaseEntity)entity.Entity).CreatedBy = currentUsername;
                            }
                        }
                        else
                        {
                            ((BaseEntity)entity.Entity).ModifiedDate = DateTime.Now;
                            if (currentUsername != "Anonymous")
                            {
                                ((BaseEntity)entity.Entity).ModifiedBy = currentUsername;
                            }
                        }

                            
                    }
                }
                else
                {
                    foreach (var entity in entities)
                    {
                        if (entity.State == EntityState.Added)
                        {
                            ((ApplicationUser)entity.Entity).CreatedDate = DateTime.Now;
                            ((ApplicationUser)entity.Entity).CreatedBy = currentUsername;
                        }
                        else
                        {
                            ((ApplicationUser)entity.Entity).ModifiedDate = DateTime.Now;
                            ((ApplicationUser)entity.Entity).ModifiedBy = currentUsername;
                        }
                    }
                }

            }
            catch (Exception)
            {

            }

        }
    }
}
