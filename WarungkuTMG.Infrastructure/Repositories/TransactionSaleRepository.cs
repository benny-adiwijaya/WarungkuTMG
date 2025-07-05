using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Domain.Entities;
using WarungkuTMG.Infrastructure.Data;

namespace WarungkuTMG.Infrastructure.Repositories
{
    public class TransactionSaleRepository : Repository<TransactionSale>, ITransactionSaleRepository
    {
        private readonly ApplicationDbContext _db;
        public TransactionSaleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TransactionSale transactionSale)
        {
            _db.TransactionSales.Update(transactionSale);
        }
    }
}
