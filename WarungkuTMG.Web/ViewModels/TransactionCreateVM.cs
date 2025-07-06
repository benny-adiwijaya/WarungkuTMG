using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using WarungkuTMG.Domain.Entities;
using WarungkuTMG.Domain.Enums;

namespace WarungkuTMG.Web.ViewModels;

public class TransactionCreateVM
{
    public TransactionSale? TransactionSale { get; set; }
    public Payment? Payment { get; set; }
    [ValidateNever]
    public IEnumerable<Product>? Products { get; set; }
}