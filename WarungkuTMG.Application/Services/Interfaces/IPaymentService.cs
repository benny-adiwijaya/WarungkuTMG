using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Application.Services.Interfaces;

public interface IPaymentService
{
    IEnumerable<Payment> GetAllPayments();
    Payment GetPaymentById(int id);
    void CreatePayment(Payment payment);
    void UpdatePayment(Payment payment);
    bool DeletePayment(int id);
}