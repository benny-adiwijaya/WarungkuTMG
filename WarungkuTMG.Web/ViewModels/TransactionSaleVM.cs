using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Web.ViewModels
{
    public class TransactionSaleVM
    {
        public TransactionSale? TransactionSale { get; set; }
        [ValidateNever]
        public List<TransactionSaleDetail>? TransactionSaleDetails { get; set; }
    }
}
