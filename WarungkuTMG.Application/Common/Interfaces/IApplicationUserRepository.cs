using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WarungkuTMG.Domain.Entities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WarungkuTMG.Application.Common.Interfaces;

public interface IApplicationUserRepository
{
    IEnumerable<ApplicationUser> GetAll(Expression<Func<ApplicationUser, bool>>? filter = null, string? includeProperties = null);
    ApplicationUser Get(Expression<Func<ApplicationUser, bool>> filter, string? includeProperties = null);
    Task<IdentityResult> Add(ApplicationUser entity);
    Task<IdentityResult> Update(ApplicationUser applicationUser);
    bool Any(Expression<Func<ApplicationUser, bool>> filter);
    Task<IdentityResult> Remove(ApplicationUser entity);
    IEnumerable<ApplicationRole> GetAllRole(Expression<Func<ApplicationRole, bool>>? filter = null, string? includeProperties = null);
    ApplicationRole GetRole(Expression<Func<ApplicationRole, bool>> filter, string? includeProperties = null);
    Task<IdentityResult> AddRole(ApplicationRole entity);
    Task<IdentityResult> AddToRole(ApplicationUser user, string role);
    bool AnyRole(Expression<Func<ApplicationRole, bool>> filter);
    Task SignOut();
    Task<SignInResult> SignIn(string userName, string password, bool isPersistent, bool lockoutOnFailure = false);
}