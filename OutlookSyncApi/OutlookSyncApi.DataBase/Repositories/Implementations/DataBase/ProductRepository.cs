using ProySuministros.DataAccess.Layer.Models;
using ProySuministros.DataAccess.Layer.Options;
using ProySuministros.DataAccess.Layer.Repositories.Implementatios.DataBase;
using ProySuministros.DataAccess.Layer.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace ProySuministros.DataAccess.Layer.Repositories.Implementations.DataBase
{
    public class ProductRepository : _BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context, IOptions<ConfigSettingsOptions> options) : base(context) {

            var opt = options.Value;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Product> InsertProducts(Product product)
        {
            DateTimeOffset now = DateTime.UtcNow;
            string owner = "Admin";
            product.Id = Guid.NewGuid().ToString();
            product.Updated = now;
            product.Created = now;
            product.CreatedBy = owner;
            product.UpdatedBy = owner;
            await _context.Productos.AddAsync(product);
            return product;
        }
    }
}
