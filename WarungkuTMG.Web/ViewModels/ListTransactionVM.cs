using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Web.ViewModels;

public class ListTransactionVM
{
    public IEnumerable<TransactionSale> ListTransactionSale { get; set; }
    public DateTime? Date { get; set; }
}