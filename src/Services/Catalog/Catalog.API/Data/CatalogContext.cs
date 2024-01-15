using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IConfiguration configuration;

        public CatalogContext(IConfiguration configuration) { 
            this.configuration = configuration;
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            
            Products = db.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

            CatalogContextSeed.SeedData(Products);

        }
        public IMongoCollection<Product> Products { get; private set; }
    }

}
