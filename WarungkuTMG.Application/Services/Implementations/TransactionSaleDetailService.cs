using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Application.Services.Interfaces;
using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Application.Services.Implementations;

public class TransactionSaleDetailService : ITransactionSaleDetailService
    {

        private readonly IUnitOfWork _unitOfWork;
        
        public TransactionSaleDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateTransactionSaleDetail(TransactionSaleDetail transactionSaleDetail)
        {
            _unitOfWork.TransactionSaleDetail.Add(transactionSaleDetail);
            _unitOfWork.Save();
        }

        public bool DeleteTransactionSaleDetail(int id)
        {
            try
            {
                TransactionSaleDetail? objFromDb = _unitOfWork.TransactionSaleDetail.Get(u => u.Id == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.TransactionSaleDetail.Remove(objFromDb);
                    _unitOfWork.Save();

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }   
        }

        public IEnumerable<TransactionSaleDetail> GetAllTransactionSaleDetails()
        {
            return _unitOfWork.TransactionSaleDetail.GetAll(includeProperties: "Product");
        }

        public TransactionSaleDetail GetTransactionSaleDetailById(int id)
        {
            return _unitOfWork.TransactionSaleDetail.Get(u => u.Id == id, includeProperties: "Product");
        }

        public void UpdateTransactionSaleDetail(TransactionSaleDetail transactionSaleDetail)
        {
            _unitOfWork.TransactionSaleDetail.Update(transactionSaleDetail);
            _unitOfWork.Save();
        }
    }