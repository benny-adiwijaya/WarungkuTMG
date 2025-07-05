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
    public class TransactionSaleDetailRepository : Repository<TransactionSaleDetail>, ITransactionSaleDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public TransactionSaleDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TransactionSaleDetail transactionSaleDetail)
        {
            _db.TransactionSaleDetails.Update(transactionSaleDetail);
        }
    }
}
