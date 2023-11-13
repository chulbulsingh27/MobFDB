using MobFDB.Models;

namespace MobFDB.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task PutProduct(Product product);
        Task<Product> PostProduct(Product product);
        Task DeleteProduct(int id);
        bool ProductExists(int id);
    }

}
