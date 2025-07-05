using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Application.Services.Interfaces;

public interface ITransactionSaleService
{
    IEnumerable<TransactionSale> GetAllTransactionSales();
    TransactionSale GetTransactionSaleById(int id);
    void CreateTransactionSale(TransactionSale transactionSale);
    void UpdateTransactionSale(TransactionSale transactionSale);
    bool DeleteTransactionSale(int id);
}