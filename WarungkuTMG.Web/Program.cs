using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Application.Services.Implementations;
using WarungkuTMG.Application.Services.Interfaces;
using WarungkuTMG.Domain.Entities;
using WarungkuTMG.Infrastructure.Data;
using WarungkuTMG.Infrastructure.Repositories;

namespace WarungkuTMG.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(
                            connectionString,
                            new MySqlServerVersion(new Version(8, 0, 20)), // specify server version
                            mySqlOptions =>
                            {
                                mySqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds); // set command timeout to (5 minutes)
                                // Optionally configure other MySQL options here
                                // e.g., mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend);
                            }
                        )
                        .EnableSensitiveDataLogging() // enable sensitive data logging
                        .EnableDetailedErrors() // enable detailed error messages
            );
            
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<ITransactionSaleService, TransactionSaleService>();
            builder.Services.AddScoped<ITransactionSaleDetailService, TransactionSaleDetailService>();
            builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            SeedDatabase();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            void SeedDatabase()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                    dbInitializer.Initialize();
                }
            }
        }
    }
}
