using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Web.ViewModels;

public class ApplicationUserVM
{
    public ApplicationUser? ApplicationUser { get; set; }
    public string? RedirectUrl { get; set; }
    public string? Role { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? RoleList { get; set; }
}