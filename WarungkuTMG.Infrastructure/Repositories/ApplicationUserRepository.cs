using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Domain.Entities;
using WarungkuTMG.Infrastructure.Data;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WarungkuTMG.Infrastructure.Repositories;

public class ApplicationUserRepository: IApplicationUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public ApplicationUserRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }
    public async Task<IdentityResult> Add(ApplicationUser entity)
    {
        return await _userManager.CreateAsync(entity);
    }

    public bool Any(Expression<Func<ApplicationUser, bool>> filter)
    {
        return _userManager.Users.Any(filter);
    }

    public ApplicationUser Get(
        Expression<Func<ApplicationUser, bool>>? filter = null,
        string? includeProperties = null)
    {
        IQueryable<ApplicationUser> query = _userManager.Users;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Include works only if navigation properties are configured and supported in Users DbSet
        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp.Trim());
            }
        }

        return query.FirstOrDefault();
    }

    public IEnumerable<ApplicationUser> GetAll(
        Expression<Func<ApplicationUser, bool>>? filter = null,
        string? includeProperties = null)
    {
        IQueryable<ApplicationUser> query = _userManager.Users;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Include works only if navigation properties are configured and supported in Users DbSet
        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp.Trim());
            }
        }

        return query.ToList();
    }

    public async Task<IdentityResult> Remove(ApplicationUser entity)
    {
        return await _userManager.DeleteAsync(entity);
    }

    public IEnumerable<ApplicationRole> GetAllRole(Expression<Func<ApplicationRole, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<ApplicationRole> query = _roleManager.Roles;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Include works only if navigation properties are configured and supported in Users DbSet
        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp.Trim());
            }
        }

        return query.ToList();
    }

    public ApplicationRole GetRole(Expression<Func<ApplicationRole, bool>> filter, string? includeProperties = null)
    {
        IQueryable<ApplicationRole> query = _roleManager.Roles;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Include works only if navigation properties are configured and supported in Users DbSet
        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp.Trim());
            }
        }

        return query.FirstOrDefault();
    }

    public async Task<IdentityResult> AddRole(ApplicationRole entity)
    {
        return await _roleManager.CreateAsync(entity);
    }

    public Task<IdentityResult> AddToRole(ApplicationUser user, string role)
    {
        return _userManager.AddToRoleAsync(user, role);   
    }

    public bool AnyRole(Expression<Func<ApplicationRole, bool>> filter)
    {
        return _roleManager.Roles.Any(filter);
    }

    public async Task SignOut()
    {
        await _signInManager.SignOutAsync();   
    }

    public async Task<SignInResult> SignIn(string userName, string password, bool isPersistent = false, bool lockoutOnFailure = false)
    {
        return await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);    
    }

    public async Task<IdentityResult> Update(ApplicationUser applicationUser)
    {
        return await _userManager.UpdateAsync(applicationUser);
    }
}
