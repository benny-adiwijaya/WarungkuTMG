using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Application.Services.Interfaces;
using WarungkuTMG.Domain.Entities;
using WarungkuTMG.Infrastructure.Data;
using WarungkuTMG.Web.ViewModels;

namespace WarungkuTMG.Web.Controllers
{
    [Authorize]
    public class TransactionSaleController : Controller
    {
        private readonly ITransactionSaleService _transactionService;

        public TransactionSaleController(ITransactionSaleService transactionService)
        {
            _transactionService = transactionService;
        }

        public IActionResult Index()
        {
            
            var transactions = _transactionService.GetAllTransactionSales();
            ListTransactionVM listTransactionVM = new ListTransactionVM
            {
                ListTransactionSale = transactions
            };
            return View(listTransactionVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TransactionSale obj)
        {
            if (ModelState.IsValid)
            {

                _transactionService.CreateTransactionSale(obj);
                TempData["success"] = "The transaction has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Update(int transactionId)
        {
            TransactionSale? obj = _transactionService.GetTransactionSaleById(transactionId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(TransactionSale obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {

               _transactionService.UpdateTransactionSale(obj);
                TempData["success"] = "The transaction has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(int transactionId)
        {
            TransactionSale? obj = _transactionService.GetTransactionSaleById(transactionId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }


        [HttpPost]
        public IActionResult Delete(TransactionSale obj)
        {
         bool deleted = _transactionService.DeleteTransactionSale(obj.Id);
            if (deleted)
            {
                TempData["success"] = "The transaction has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the transaction.";
            }
            return View();
        }
        
        [HttpPost]
        public IActionResult GetTransactionsByDate(DateTime date)
        {
            ListTransactionVM obj = new();
            if (date == DateTime.MinValue)
            {
                obj = new()
                {
                    ListTransactionSale = _transactionService.GetAllTransactionSales(),
                };
            }
            else
            {
                obj = new()
                {
                    Date = date,
                    ListTransactionSale = _transactionService.GetTransactionsByDate(date),
                };
            }
            

            return PartialView("_TransactionList",obj);
        }
    }
}
