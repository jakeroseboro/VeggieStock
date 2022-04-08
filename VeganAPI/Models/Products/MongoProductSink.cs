using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VeganAPI.Configuration;

namespace VeganAPI.Models.Products;

public class MongoProductSink : IProductMongoSink
{
    private readonly IMongoCollection<Product> _products;

    public MongoProductSink(IMongoDbConnectionSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _products = database.GetCollection<Product>(settings.ProductsCollectionName);

        var indexBuilder = Builders<Product>.IndexKeys;
        var indexModel = new CreateIndexModel<Product>(indexBuilder
            .Ascending(x => x.Id)
            .Ascending(x => x.Name), new CreateIndexOptions{ Unique = true }
        );
        _products.Indexes.CreateOneAsync(indexModel, cancellationToken: CancellationToken.None);

    }
    public async Task<ActionResult<Product>> CreateProduct(Product product, CancellationToken cancellationToken = default)
    {
        await _products.InsertOneAsync(product, new InsertOneOptions(),  cancellationToken);
        return product;
    }

    public async Task<ActionResult<Product>> UpdateProduct(ProductUpdateOptions updateOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResult<Product>> UpdateProductSightings(ProductUpdateOptions updateOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}