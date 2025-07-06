using System.ComponentModel.DataAnnotations;

namespace WarungkuTMG.Web.ViewModels;

public class LoginVM
{
    [Required]
    public required string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    public bool RememberMe { get; set; }

    public string? RedirectUrl { get; set; }
}