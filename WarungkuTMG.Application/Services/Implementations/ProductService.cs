using Microsoft.AspNetCore.Hosting;
using WarungkuTMG.Application.Common.Interfaces;
using WarungkuTMG.Application.Services.Interfaces;
using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Application.Services.Implementations;

public class ProductService : IProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public void CreateProduct(Product product)
        {
            if (product.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "ProductImages");

                using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                product.Image.CopyTo(fileStream);

                product.ImageUrl = Path.Combine("/images", "ProductImages", fileName);
            }
            else
            {
                product.ImageUrl = "https://placehold.co/600x400";
            }

            _unitOfWork.Product.Add(product);
            _unitOfWork.Save();
        }

        public bool DeleteProduct(int id)
        {
            try
            {
                Product? objFromDb = _unitOfWork.Product.Get(u => u.Id == id);
                if (objFromDb is not null)
                {
                    if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    _unitOfWork.Product.Remove(objFromDb);
                    _unitOfWork.Save();

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }   
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _unitOfWork.Product.GetAll();
        }

        public Product GetProductById(int id)
        {
            return _unitOfWork.Product.Get(u => u.Id == id);
        }

        public void UpdateProduct(Product product)
        {
            if (product.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "ProductImages");

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                product.Image.CopyTo(fileStream);

                product.ImageUrl = Path.Combine("/images", "ProductImages", fileName);
            }

            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();
        }
    }