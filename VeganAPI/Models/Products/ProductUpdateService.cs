using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Products;

public class ProductUpdateService : IProductUpdateService
{
    private readonly IProductMongoSink _sink;

    public ProductUpdateService(IProductMongoSink sink)
    {
        _sink = sink;
    }
    public async Task<ActionResult<Product>> UpdateProduct(ProductUpdateOptions updateOptions, CancellationToken cancellationToken = default)
    {
        try
        {
            var results = await _sink.UpdateProduct(updateOptions, cancellationToken);
            return results;
        }
        catch (Exception e)
        {
            return new ObjectResult(new {error = $"Unable to update product due to {e}"})
            {
                StatusCode = 500
            };
        }
        
    }

    public async Task<ActionResult<Product>> UpdateProductSighting(ProductUpdateOptions updateOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}