using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Application.Common.Utility;
using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Infrastructure.Data;

public class DbInitializer : IDbInitializer
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _db;

    public DbInitializer(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationDbContext db)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _db = db;
    }

    public void Initialize()
    {
        try
        {
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }
            if (!_roleManager.RoleExistsAsync(SD.Role_Administrator).GetAwaiter().GetResult())
            {
                var role = new ApplicationRole
                {
                    Name = SD.Role_Administrator,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now
                };
                _roleManager.CreateAsync(role).GetAwaiter().GetResult();;
            }

            if (!_roleManager.RoleExistsAsync(SD.Role_User).GetAwaiter().GetResult())
            {
                var role = new ApplicationRole
                {
                    Name = SD.Role_User,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now
                };
                _roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }

            if (_userManager.FindByNameAsync("admin").GetAwaiter().GetResult() == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@pttmg.com",
                    Name = "Smith Wong",
                    NormalizedUserName = "ADMIN@PTTMG.COM",
                    NormalizedEmail = "ADMIN@PTTMG.COM",
                    PhoneNumber = "1112223333",
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now

                };

                var result = _userManager.CreateAsync(user, "Admin123*").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, SD.Role_Administrator).GetAwaiter().GetResult();
                }
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }
}