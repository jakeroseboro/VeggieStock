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
                .Ascending(x => x.Name)
                .Ascending(x => x.Brand),
                new CreateIndexOptions{ Unique = true }
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
        
        //Get the sighting that matches the store and address 
        var sighting = sightings.FirstOrDefault(x => x.Store.Name == updateOptions.Sighting.Store.Name && x.ZipCode == updateOptions.Sighting.ZipCode && x.Street == updateOptions.Sighting.Street);
        
        //If this sighting does not exist, add a new one
        if (sighting == null)
        {
            product.Sightings.Add(updateOptions.Sighting);
            if (!product.ZipCodes.Contains(updateOptions.Sighting.ZipCode))
            {
                product.ZipCodes.Add(updateOptions.Sighting.ZipCode);
            }
            updates.Add(update.Set(x => x.Sightings, product.Sightings)); 
            updates.Add(update.Set(x => x.ZipCodes, product.ZipCodes)); 
        }
        //If this sighting does exist, update it
        else
        {
            sighting.Seen = updateOptions.Sighting.Seen;
            sighting.SpottedBy = updateOptions.Sighting.SpottedBy;
            updates.Add(update.Set(x => x.Sightings, sightings));
        }
        updates.Add(update.Set(x => x.LastSpotted, updateOptions.Sighting.Seen));

        if (updateOptions.IsDeleted.HasValue)
        {
            updates.Add(update.Set(x => x.IsDeleted, updateOptions.IsDeleted));
        }

        var result = await _products.FindOneAndUpdateAsync(filter, update.Combine(updates),
            new FindOneAndUpdateOptions<Product> {IsUpsert = false, ReturnDocument = ReturnDocument.After},
            cancellationToken);

        return result;
    }
}