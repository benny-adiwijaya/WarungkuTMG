using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarungkuTMG.Domain.Enums;

namespace WarungkuTMG.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }

        public int TransactionSaleId { get; set; }
        [ValidateNever]
        public TransactionSale? TransactionSale { get; set; }

        public PaymentType PaymentType { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal? CashReceived { get; set; }  // For cash payments only
        [Column(TypeName = "decimal(18,0)")]
        public decimal? Change { get; set; }        // For cash payments only

        public string? EvidenceNumber { get; set; } // For non cash
    }
}
