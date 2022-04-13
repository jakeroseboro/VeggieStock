using Microsoft.AspNetCore.Mvc;
using VeganAPI.Models.Products.Subclasses;

namespace VeganAPI.Models.Products;

public class ProductCreationService : IProductCreationService
{
    private readonly IProductMongoSink _sink;

    public ProductCreationService(IProductMongoSink sink)
    {
        _sink = sink;
    }
    
    public async Task<ActionResult<Product>> CreateProduct(NewProduct newProduct, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                CoverImage = newProduct.CoverImage,
                Brand = newProduct.Brand,
                Type = newProduct.Type,
                CreatedBy = newProduct.CreatedBy,
                Name = newProduct.Name,
                IsDeleted = false,
                LastSpotted = DateTime.Now,
                Sightings = new List<Sighting> {newProduct.Sighting},
                ZipCodes = new List<int> {newProduct.Sighting.ZipCode}
            };

            var result = await _sink.CreateProduct(product, cancellationToken);
            return result;
        }
        catch (Exception e)
        {
            return new ObjectResult(new {error = $"Unable to create product due to {e}"})
            {
                StatusCode = 500
            };
        }
    }
}