using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WarungkuTMG.Domain.Enums;

namespace WarungkuTMG.Domain.Entities
{
    public class TransactionSale : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string UserLogin { get; set; }
        [Required]
        [StringLength(100)]
        public required string CustomerName { get; set; }
        [Column(TypeName = "decimal(18,0)")]

        public decimal DiscountPercentage { get; set; }
        [Column(TypeName = "decimal(18,0)")]

        public decimal VoucherAmount { get; set; }
        [Column(TypeName = "decimal(18,0)")]

        public decimal Total { get; set; }
        [Column(TypeName = "decimal(18,0)")]

        public decimal DiscountAmount => Total * (DiscountPercentage / 100);
        [Column(TypeName = "decimal(18,0)")]
        public decimal GrandTotal => Total - DiscountAmount - VoucherAmount;

        [ValidateNever]
        public virtual List<TransactionSaleDetail>? Details { get; set; }
        [ValidateNever]
        public virtual Payment? Payment { get; set; }
    }
}
