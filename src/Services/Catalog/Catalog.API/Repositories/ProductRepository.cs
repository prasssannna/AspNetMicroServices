using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }
        public async Task AddAsync(Product product)
        {
            await this.catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var product = await this.catalogContext.Products.Find(f => f.Id == id).FirstOrDefaultAsync();

            if (product != null)
            {
                var result = await this.catalogContext.Products.DeleteOneAsync(filter: f => f.Id == product.Id);

                if (result.IsAcknowledged && result.DeletedCount > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await this.catalogContext.Products.Find(f => true).ToListAsync();
        }

        public async Task<Product> GetProductByCategoryAsync(string category)
        {
            return await this.catalogContext.Products.Find(f => f.Category == category).FirstOrDefaultAsync();

        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await this.catalogContext.Products.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            return await this.catalogContext.Products.Find(f => f.Name == name).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var result = await this.catalogContext.Products.
                ReplaceOneAsync(filter: f => f.Id == product.Id, replacement: product);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return true;
            }
            return false;
        }
    }
}
