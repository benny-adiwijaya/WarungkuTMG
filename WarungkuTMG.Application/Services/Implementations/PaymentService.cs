using Microsoft.AspNetCore.Hosting;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Application.Services.Interfaces;
using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Application.Services.Implementations;

public class PaymentService : IPaymentService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PaymentService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public void CreatePayment(Payment payment)
        {
            
            _unitOfWork.Payment.Add(payment);
            _unitOfWork.Save();
        }

        public bool DeletePayment(int id)
        {
            try
            {
                Payment? objFromDb = _unitOfWork.Payment.Get(u => u.Id == id);
                if (objFromDb is not null)
                {
                    
                    _unitOfWork.Payment.Remove(objFromDb);
                    _unitOfWork.Save();

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }   
        }

        public IEnumerable<Payment> GetAllPayments()
        {
            return _unitOfWork.Payment.GetAll();
        }

        public Payment GetPaymentById(int id)
        {
            return _unitOfWork.Payment.Get(u => u.Id == id);
        }

        public void UpdatePayment(Payment payment)
        {
            
            _unitOfWork.Payment.Update(payment);
            _unitOfWork.Save();
        }
    }