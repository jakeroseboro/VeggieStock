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
            filter &= builder.Eq(x => x.Name, queryOptions.Name);
        }

        if (!string.IsNullOrWhiteSpace(queryOptions.StoreName))
        {
            filter &= builder.Where(x => x.Sightings.Any(y => y.Store.Name == queryOptions.StoreName));
        }

        var zipcodes = queryOptions.ZipCodes ?? new List<int>();

        if (zipcodes.Any())
        {
            filter &= builder.Where(x => x.ZipCodes.Any(y => zipcodes.Contains(y)));
        }

        if (!string.IsNullOrWhiteSpace(queryOptions.CreatedBy))
        {
            filter &= builder.Eq(x => x.CreatedBy, queryOptions.CreatedBy);
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
            return new ObjectResult(new {error = $"Unable to find product"})
            {
                StatusCode = 500
            };
        }

        return product;
    }
}