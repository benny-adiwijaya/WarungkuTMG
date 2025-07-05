using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Infrastructure.Data;

namespace WarungkuTMG.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IProductRepository Product { get; private set; }
        public ITransactionSaleRepository TransactionSale { get; private set; }
        public ITransactionSaleDetailRepository TransactionSaleDetail { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Product = new ProductRepository(_db);
            TransactionSale = new TransactionSaleRepository(_db);
            TransactionSaleDetail = new TransactionSaleDetailRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
