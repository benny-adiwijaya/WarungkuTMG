using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Application.Common.Interfaces;

public interface IPaymentRepository: IRepository<Payment>
{
    void Update(Payment payment);
}
