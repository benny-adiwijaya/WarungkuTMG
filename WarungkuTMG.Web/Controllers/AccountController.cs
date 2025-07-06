using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Application.Common.Utility;
using WarungkuTMG.Application.Services.Interfaces;
using WarungkuTMG.Domain.Entities;
using WarungkuTMG.Web.ViewModels;

namespace WarungkuTMG.Web.Controllers
{
    public class AccountController : Controller
    {
        
        // private readonly UserManager<ApplicationUser> _userManager;
        // private readonly SignInManager<ApplicationUser> _signInManager;
        // private readonly RoleManager<ApplicationRole> _roleManager;
        
        private readonly IApplicationUserService _applicationUserService;

        public AccountController(
            // UserManager<ApplicationUser> userManager,
            // RoleManager<ApplicationRole> roleManager,
            // SignInManager<ApplicationUser> signInManager, 
            IApplicationUserService applicationUserService)
        {
            // _roleManager = roleManager;
            // _userManager = userManager;
            // _signInManager = signInManager;
            _applicationUserService = applicationUserService;
        }

        public IActionResult Login(string returnUrl=null)
        {

            returnUrl??= Url.Content("~/");

            LoginVM loginVM = new ()
            {
                RedirectUrl = returnUrl,
                UserName = null,
                Password = null
            };

            return View(loginVM);
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _applicationUserService
                    .SignIn(loginVM.UserName, loginVM.Password, loginVM.RememberMe, lockoutOnFailure:false);


                if (result.Succeeded)
                {
                    var user = _applicationUserService.FindByUserName(loginVM.UserName);
                    if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(loginVM.RedirectUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _applicationUserService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        
        [Authorize(Roles = SD.Role_Administrator)]
        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (!_applicationUserService.RoleExist(SD.Role_Administrator))
            {
                _applicationUserService.CreateRole(new ApplicationRole()
                {
                    Name = SD.Role_Administrator,
                }).Wait();
                _applicationUserService.CreateRole(new ApplicationRole()
                {
                    Name = SD.Role_User,
                }).Wait();
            }

            RegisterVM registerVM = new ()
            {
                RoleList = _applicationUserService.GetAllRoles().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                RedirectUrl = returnUrl,
                UserName = null,
                Email = null,
                Password = null,
                ConfirmPassword = null,
                Name = null
            };

            return View(registerVM);
        }
        
        [Authorize(Roles = SD.Role_Administrator)]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = registerVM.Name,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    ImageUrl = registerVM.ImageUrl ?? "https://placehold.co/600x400",
                    Image = registerVM.Image ?? null,
                    UserName = registerVM.UserName,
                    CreatedDate = DateTime.Now
                };

                var result = await _applicationUserService.CreateUser(user, registerVM.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        await _applicationUserService.AddToRole(user, registerVM.Role);
                    }
                    else
                    {
                        await _applicationUserService.AddToRole(user, SD.Role_User);
                    }
                    
                    if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return LocalRedirect(registerVM.RedirectUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                } 
            }
            registerVM.RoleList = _applicationUserService.GetAllRoles().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });

            return View(registerVM);
        }
        
        [Authorize]
        public IActionResult Index()
        {
            var users = _applicationUserService.GetAllUsers();
            return View(users);
        }
        
        public IActionResult Update(string applicationUserId)
        {
            ApplicationUser? user = _applicationUserService.GetUserById(applicationUserId);;
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var role = user.UserRoles?.FirstOrDefault()?.Role?.Name;
            var obj = new ApplicationUserVM
            {
                ApplicationUser = user,
                Role = role,
                RoleList = _applicationUserService.GetAllRoles().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                })
            };
            return View(obj);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(ApplicationUserVM obj)
        {
            if (ModelState.IsValid && obj.ApplicationUser.Id is not null)
            {
                var existingUser = _applicationUserService.GetUserById(obj.ApplicationUser.Id);
                if (existingUser is not null)
                {
                    existingUser.Name = obj.ApplicationUser.Name;
                    existingUser.Email = obj.ApplicationUser.Email;
                    existingUser.PhoneNumber = obj.ApplicationUser.PhoneNumber;
                    existingUser.NormalizedEmail = obj.ApplicationUser.Email?.ToUpper();
                    existingUser.ImageUrl = obj.ApplicationUser.ImageUrl;
                    existingUser.Image = obj.ApplicationUser.Image;
                    
                    var result = await _applicationUserService.UpdateUser(existingUser);
                    if (result.Succeeded)
                    {
                        TempData["success"] = "The user has been updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["error"] = "Failed to update the user.";
                    }
                }
                else
                {
                    TempData["error"] = "Failed to update the user. User not found.";
                }
            }

            obj.RoleList = _applicationUserService.GetAllRoles().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });
            return View(obj);
        }
        
        public IActionResult Delete(string applicationUserId)
        {
            ApplicationUser? obj = _applicationUserService.GetUserById(applicationUserId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser obj)
        {
            var existingUser = _applicationUserService.GetUserById(obj.Id);
            if (existingUser is not null)
            {
                var result = await _applicationUserService.DeleteUser(existingUser);
                if (result.Succeeded)
                {
                    TempData["success"] = "The user has been deleted successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Failed to delete the user.";
                }
            }
            else
            {
                TempData["error"] = "Failed to delete the user. User not found.";
            }
            
            
            return View();
        }
    }
}
