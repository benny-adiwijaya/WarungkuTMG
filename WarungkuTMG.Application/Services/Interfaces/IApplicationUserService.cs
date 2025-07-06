using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WarungkuTMG.Domain.Entities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WarungkuTMG.Application.Services.Interfaces;

public interface IApplicationUserService
{
    IEnumerable<ApplicationUser> GetAllUsers();
    ApplicationUser GetUserById(string id);
    Task<IdentityResult> CreateUser(ApplicationUser user, string password);
    Task<IdentityResult> UpdateUser(ApplicationUser user);
    Task<IdentityResult> DeleteUser(ApplicationUser user);
    
    Task<SignInResult> SignIn(string userName, string password, bool isPersistent = false, bool lockoutOnFailure = false);
    Task<IdentityResult> AddToRole(ApplicationUser user, string role);
    ApplicationUser FindByUserName(string userName);
    Task SignOut();
    bool RoleExist(string roleName);
    Task<IdentityResult> CreateRole(ApplicationRole role);
    IEnumerable<ApplicationRole> GetAllRoles();
}