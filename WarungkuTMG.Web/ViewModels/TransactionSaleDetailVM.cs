using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Web.ViewModels
{
    public class TransactionSaleDetailVM
    {
        public TransactionSaleDetail? TransactionSaleDetail { get; set; }
        [ValidateNever]
        public List<Product>? Products { get; set; }
    }
}
