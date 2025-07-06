using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Application.Services.Interfaces;
using WarungkuTMG.Domain.Entities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WarungkuTMG.Application.Services.Implementations;

public class ApplicationUserService : IApplicationUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ApplicationUserService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    
    public IEnumerable<ApplicationUser> GetAllUsers()
    {
        var users = _unitOfWork.ApplicationUser.GetAll(includeProperties: "UserRoles.Role");
        return users;   
    }

    public ApplicationUser GetUserById(string id)
    {
        return _unitOfWork.ApplicationUser.Get(q => q.Id == id, includeProperties: "UserRoles.Role");
    }

    public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
    {
        if (user.Image != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(user.Image.FileName);
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "UserImages");

            using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
            user.Image.CopyTo(fileStream);

            user.ImageUrl = Path.Combine("/images", "UserImages", fileName);
        }
        else
        {
            user.ImageUrl = "https://placehold.co/600x400";
        }
        
        return await _unitOfWork.ApplicationUser.Add(user);
    }

    public async Task<IdentityResult> UpdateUser(ApplicationUser user)
    {
        if (user.Image != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(user.Image.FileName);
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "UserImages");

            if (!string.IsNullOrEmpty(user.ImageUrl))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, user.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
            user.Image.CopyTo(fileStream);

            user.ImageUrl = Path.Combine("/images", "UserImages", fileName);
        }

        return await _unitOfWork.ApplicationUser.Update(user);
    }

    public async Task<IdentityResult> DeleteUser(ApplicationUser user)
    {
        ApplicationUser? objFromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == user.Id);
        if (objFromDb is not null)
        {
            if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            
            return await _unitOfWork.ApplicationUser.Remove(user);
        }
        return IdentityResult.Failed();
    }

    public async Task<SignInResult> SignIn(string userName, string password, bool isPersistent = false, bool lockoutOnFailure = false)
    {
        return await _unitOfWork.ApplicationUser.SignIn(userName, password, isPersistent, lockoutOnFailure);
    }

    public async Task<IdentityResult> AddToRole(ApplicationUser user, string role)
    {
        return await _unitOfWork.ApplicationUser.AddToRole(user, role);  
    }

    public ApplicationUser FindByUserName(string userName)
    {
        return _unitOfWork.ApplicationUser.Get(q => q.UserName == userName, includeProperties: "UserRoles.Role");
    }

    public async Task SignOut()
    {
        await _unitOfWork.ApplicationUser.SignOut();
    }

    public bool RoleExist(string roleName)
    {
        return _unitOfWork.ApplicationUser.AnyRole(q => q.Name == roleName);
    }

    public async Task<IdentityResult> CreateRole(ApplicationRole role)
    {
        return await _unitOfWork.ApplicationUser.AddRole(role);
    }

    public IEnumerable<ApplicationRole> GetAllRoles()
    {
        return _unitOfWork.ApplicationUser.GetAllRole();
    }
}