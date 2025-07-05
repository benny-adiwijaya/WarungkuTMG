using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WarungkuTMG.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
        [StringLength(250)]
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public ICollection<ApplicationUserRole>? UserRoles { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        public DateTime? DisabledDate { get; set; }
        public bool IsDisabled { get; set; }
        [StringLength(100)]
        public string? DisabledBy { get; set; }
    }
}
