using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarungkuTMG.Domain.Entities
{
    public class TransactionSaleDetail : BaseEntity
    {
        public int Id { get; set; }
        public int TransactionSaleId { get; set; }
        [ValidateNever]
        public TransactionSale? TransactionSale { get; set; }

        public int ProductId { get; set; }
        [ValidateNever]
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal Subtotal => Quantity * Price;
    }
}
