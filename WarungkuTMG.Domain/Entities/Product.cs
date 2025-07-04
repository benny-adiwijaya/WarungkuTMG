using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarungkuTMG.Domain.Entities
{
    public class Product : BaseEntity
    {
        public int ProductId { get; set; }
        [StringLength(100)]
        public required string Name { get; set; }
        [StringLength(250)]
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [StringLength(250)]
        public string? ImageUrl { get; set; }
    }
}
