using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WarungkuTMG.Domain.Entities
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        [StringLength(100)]
        public required string Name { get; set; }
        [StringLength(250)]
        public string? Description { get; set; }
        [Range(0, 9999999999)]
        public decimal Price { get; set; }
        [Display(Name = "Image Url")]
        [StringLength(250)]
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
