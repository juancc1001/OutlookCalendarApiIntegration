using ProySuministros.DataAccess.Layer.Models;
using ProySuministros.DataAccess.Layer.Repository.Interfaces;

namespace ProySuministros.DataAccess.Layer.Repositories.Interfaces
{
    public interface IProductRepository : _IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> InsertProducts(Product product);
    }
}
