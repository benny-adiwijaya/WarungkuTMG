using Microsoft.AspNetCore.Mvc;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Domain.Entities;
using WarungkuTMG.Infrastructure.Data;

namespace WarungkuTMG.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _productRepository;
        public ProductController(IUnitOfWork productRepository)
        {
            _productRepository = productRepository;
        }
        public IActionResult Index()
        {
            // Retrieve products from the database
            var products = _productRepository.Product.GetAll();
            return View(products);
        }

        public IActionResult Create()
        {
            // Return the view for creating a new product
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {

            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {

                _productRepository.Product.Add(obj);
                TempData["success"] = "The product has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Update(int productId)
        {
            Product? obj = _productRepository.Product.Get(q => q.Id == productId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
    }
}
