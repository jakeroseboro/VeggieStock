using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Products;

public class ProductUpdateService : IProductUpdateService
{
    private readonly IProductMongoSink _sink;
    private readonly IMongoProductSource _source;

    public ProductUpdateService(IProductMongoSink sink, IMongoProductSource source)
    {
        _sink = sink;
        _source = source;
    }
    public async Task<ActionResult<Product>> UpdateProduct(ProductUpdateOptions updateOptions, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _source.GetProductById(updateOptions.Id, cancellationToken);
            if (product.Value == null)
            {
                return new ObjectResult(new {error = $"Unable to find product"})
                {
                    StatusCode = 500
                };
            }
            
            var results = await _sink.UpdateProduct(product.Value, updateOptions, cancellationToken);
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
}