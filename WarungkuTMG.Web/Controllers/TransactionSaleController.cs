using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Application.Services.Interfaces;
using WarungkuTMG.Domain.Entities;
using WarungkuTMG.Domain.Enums;
using WarungkuTMG.Infrastructure.Data;
using WarungkuTMG.Web.ViewModels;

namespace WarungkuTMG.Web.Controllers
{
    [Authorize]
    public class TransactionSaleController : Controller
    {
        private readonly ITransactionSaleService _transactionService;
        private readonly ITransactionSaleDetailService _transactionDetailService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;

        public TransactionSaleController(ITransactionSaleService transactionService, 
            IApplicationUserService applicationUserService,
            IProductService productService,
            ITransactionSaleDetailService transactionDetailService,
            IPaymentService paymentService)
        {
            _transactionService = transactionService;
            _applicationUserService = applicationUserService;
            _productService = productService;
            _transactionDetailService = transactionDetailService;
            _paymentService = paymentService;
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
            string fullName = "";
            var userName = User.Identity?.Name ?? "Anonymous";
            var user = _applicationUserService.FindByUserName(userName);
            if (user != null)
            {
                fullName = user.Name; // assuming you have this property
            }
            // var products = _productService.GetProductById(1);
            // var dummyDetail = new TransactionSaleDetail
            // {
            //     ProductId = products.Id,
            //     Product = products,
            //     Quantity = 1,
            //     Price = products.Price,
            // };
            TransactionCreateVM obj = new()
            {
                TransactionSale = new TransactionSale
                {
                    UserLogin = fullName,
                    CustomerName = null,
                    Details = new List<TransactionSaleDetail>(),
                },
                Payment = new Payment()
            };
            
            // obj.TransactionSale.Details.Add(dummyDetail);
            obj.TransactionSale.Total = obj.TransactionSale.Details.Sum(x => x.Price * x.Quantity);
            obj.Products = _productService.GetAllProducts();
            return View(obj);
        }

        [HttpPost]
        public IActionResult Create(TransactionCreateVM model, string? CashAmount, string? EvidenceNumber)
        {
            ModelState.Remove("Payment.TransactionSale");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var details = model.TransactionSale.Details;
                model.TransactionSale.Details = null;
                // Call your service to create the transaction
                _transactionService.CreateTransactionSale(model.TransactionSale);
                if (details != null)
                {
                    foreach (var detail in details)
                    {
                        detail.TransactionSaleId = model.TransactionSale.Id;
                        detail.Product = null;
                        _transactionDetailService.CreateTransactionSaleDetail(detail);
                    }
                }

                model.Payment.TransactionSaleId = model.TransactionSale.Id;
                _paymentService.CreatePayment(model.Payment);


                // Redirect to success page or transaction list
                TempData["success"] = "The transaction has been deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle error
                ModelState.AddModelError("", "Error creating transaction: " + ex.Message);
                TempData["error"] = "Failed to create the transaction. Please try again later. .";
                return View(model);
            }
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
        
        public IActionResult Detail(int transactionId)
        {
            
            var transactions = _transactionService.GetTransactionSaleById(transactionId);
            TransactionDetailVM transactionDetailVM = new TransactionDetailVM
            {
                 TransactionSale = transactions,
            };
            return View(transactionDetailVM);
        }
    }
}
