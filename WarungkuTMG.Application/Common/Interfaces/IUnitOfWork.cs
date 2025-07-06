using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarungkuTMG.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }
        ITransactionSaleRepository TransactionSale { get; }
        ITransactionSaleDetailRepository TransactionSaleDetail { get; }
        IApplicationUserRepository ApplicationUser { get; }
        void Save();
    }
}
