using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Application.Services.Interfaces;
using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Application.Services.Implementations;

public class TransactionSaleService : ITransactionSaleService
    {

        private readonly IUnitOfWork _unitOfWork;
        
        public TransactionSaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateTransactionSale(TransactionSale transactionSale)
        {
            _unitOfWork.TransactionSale.Add(transactionSale);
            _unitOfWork.Save();
        }

        public bool DeleteTransactionSale(int id)
        {
            try
            {
                TransactionSale? objFromDb = _unitOfWork.TransactionSale.Get(u => u.Id == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.TransactionSale.Remove(objFromDb);
                    _unitOfWork.Save();

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }   
        }

        public IEnumerable<TransactionSale> GetAllTransactionSales()
        {
            var result = _unitOfWork.TransactionSale.GetAll();
            return result;  
        }

        public IEnumerable<TransactionSale> GetTransactionsByDate(DateTime date)
        {
            var result = _unitOfWork.TransactionSale.GetAll(q => q.CreatedDate.Value.Date == date);
            return result;  
        }

        public TransactionSale GetTransactionSaleById(int id)
        {
            var result = _unitOfWork.TransactionSale.Get(u => u.Id == id, includeProperties: "Details.Product");
            return result; 
        }

        public void UpdateTransactionSale(TransactionSale transactionSale)
        {
            _unitOfWork.TransactionSale.Update(transactionSale);
            _unitOfWork.Save();
        }
    }