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

    public async Task<ActionResult<Product>> UpdateProduct(Product product, ProductUpdateOptions updateOptions, CancellationToken cancellationToken = default)
    {
        var update = Builders<Product>.Update;
        var updates = new List<UpdateDefinition<Product>>();
        var filter = Builders<Product>.Filter.Eq(x => x.Id, updateOptions.Id);

        var sightings = product.Sightings;
        var sighting = sightings.FirstOrDefault(x => x.Store == updateOptions.Sighting.Store && x.ZipCode == updateOptions.Sighting.ZipCode);
        if (sighting == null)
        {
            product.Sightings.Add(updateOptions.Sighting);
            product.ZipCodes.Add(updateOptions.Sighting.ZipCode);
            updates.Add(update.Set(x => x.Sightings, product.Sightings)); 
            updates.Add(update.Set(x => x.ZipCodes, product.ZipCodes)); 
        }
        else
        {
            updates.Add(update.Set(x => x.Sightings.FirstOrDefault(y => y == sighting), updateOptions.Sighting));
        }

        var result = await _products.FindOneAndUpdateAsync(filter, update.Combine(updates),
            new FindOneAndUpdateOptions<Product> {IsUpsert = false, ReturnDocument = ReturnDocument.After},
            cancellationToken);

        return result;
    }
}