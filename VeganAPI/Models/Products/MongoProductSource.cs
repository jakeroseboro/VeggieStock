using Microsoft.AspNetCore.Mvc;
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
    
    public async Task<ActionResult<IList<Product>>> GetProducts(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default)
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

        var zipcodes = queryOptions.ZipCode ?? new List<int>();

        if (zipcodes.Any())
        {
            filter &= builder.Where(x => x.ZipCodes.All(y => zipcodes.Contains(y)));
        }

        var products = await _products.Find(filter).ToListAsync(cancellationToken);

        return products;
    }

    public async Task<ActionResult<Product>> GetProductById(Guid id, CancellationToken cancellationToken = default)
    {
        var builder = Builders<Product>.Filter.Eq(x => x.Id, id);
        var productList = await _products.Find(builder).ToListAsync(cancellationToken);
        var product = productList.FirstOrDefault();

        if (product == null)
        {
            return new ActionResult<Product>(new NotFoundResult());
        }

        return product;
    }
}