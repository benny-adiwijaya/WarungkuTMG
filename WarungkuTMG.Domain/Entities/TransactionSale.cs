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
        [Range(0, 9999999999)]

        public decimal DiscountPercentage { get; set; }
        [Range(0, 9999999999)]

        public decimal VoucherAmount { get; set; }
        [Range(0, 9999999999)]

        public decimal Total { get; set; }
        [Range(0, 9999999999)]

        public decimal DiscountAmount => Total * (DiscountPercentage / 100);
        [Range(0, 9999999999)]
        public decimal GrandTotal => Total - DiscountAmount - VoucherAmount;

        [Required]
        public PaymentType PaymentType { get; set; }

        [ValidateNever]
        public virtual List<TransactionSaleDetail>? Details { get; set; }
    }
}
