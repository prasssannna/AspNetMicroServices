using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllProductsAsync();
        public Task<Product> GetProductByIdAsync(string id);
        public Task<Product> GetProductByNameAsync(string name);
        public Task<Product> GetProductByCategoryAsync(string name);
        public Task AddAsync(Product product);
        public Task<bool> UpdateAsync(Product product);
        public Task<bool> DeleteAsync(string id);
    }
}
