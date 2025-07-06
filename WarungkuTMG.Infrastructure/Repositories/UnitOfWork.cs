using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Domain.Entities;
using WarungkuTMG.Infrastructure.Data;

namespace WarungkuTMG.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IProductRepository Product { get; private set; }
        public ITransactionSaleRepository TransactionSale { get; private set; }
        public ITransactionSaleDetailRepository TransactionSaleDetail { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IPaymentRepository Payment { get; private set; }

        public UnitOfWork(ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            Product = new ProductRepository(_db);
            Payment = new PaymentRepository(_db);
            TransactionSale = new TransactionSaleRepository(_db);
            TransactionSaleDetail = new TransactionSaleDetailRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_userManager, _roleManager, _signInManager);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
