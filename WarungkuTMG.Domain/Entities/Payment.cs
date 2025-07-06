using System;
using System.Collections.Generic;
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
        public TransactionSale TransactionSale { get; set; } = null!;

        public PaymentType PaymentType { get; set; } 

        public decimal Amount { get; set; }

        public decimal? CashReceived { get; set; }  // For cash payments only

        public decimal? Change { get; set; }        // For cash payments only

        public string? EvidenceNumber { get; set; } // For non cash
    }
}
