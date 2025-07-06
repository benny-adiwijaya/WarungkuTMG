using WarungkuTMG.Domain.Entities;

namespace WarungkuTMG.Application.Services.Interfaces;

public interface IProductService
{
    IEnumerable<Product> GetAllProducts();
    Product GetProductById(int id);
    void CreateProduct(Product product);
    void UpdateProduct(Product product);
    bool DeleteProduct(int id);
}