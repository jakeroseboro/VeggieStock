using MongoDB.Driver;
using VeganAPI.Configuration;

namespace VeganAPI.Models.Products;

public class MongoProductSource : IMongoProductSource
{
    private readonly IMongoCollection<Product> _products;
    
    public MongoProductSource(IMongoDbConnectionSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _products = database.GetCollection<Product>(settings.ProductsCollectionName);
    }
    
    public async Task<IList<Product>> GetProducts(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;
        if (!string.IsNullOrWhiteSpace(queryOptions.Name))
        {
            filter &= builder.Eq(x => x.Name.ToLowerInvariant(), queryOptions.Name);
        }

        if (!string.IsNullOrWhiteSpace(queryOptions.StoreName))
        {
            filter &= builder.Where(x => x.Sightings.All(y => y.Store.Name == queryOptions.StoreName));
        }

        if (queryOptions.ZipCode.HasValue && queryOptions.Distance.HasValue)
        {
            filter &= builder.Where(x => x.ZipCodes.All(Math.Sqrt(
                
                )))
        }
    }

    public async Task<Product> GetProductById(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}