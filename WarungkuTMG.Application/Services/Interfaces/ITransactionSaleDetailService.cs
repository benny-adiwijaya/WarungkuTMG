using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Application.Services.Interfaces;

public interface ITransactionSaleDetailService
{
    IEnumerable<TransactionSaleDetail> GetAllTransactionSaleDetails();
    TransactionSaleDetail GetTransactionSaleDetailById(int id);
    void CreateTransactionSaleDetail(TransactionSaleDetail transactionSaleDetail);
    void UpdateTransactionSaleDetail(TransactionSaleDetail transactionSaleDetail);
    bool DeleteTransactionSaleDetail(int id);
}