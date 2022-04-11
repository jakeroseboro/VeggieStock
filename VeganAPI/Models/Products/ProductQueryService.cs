using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Products;

public class ProductQueryService : IProductQueryService
{
    private readonly IMongoProductSource _source;

    public ProductQueryService(IMongoProductSource source)
    {
        _source = source;
    }

    public async Task<ActionResult<IList<Product>>> GetProducts(ProductQueryOptions queryOptions,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var results = await _source.GetProducts(queryOptions, cancellationToken);
            return results;
        }
        catch (Exception e)
        {
            return new ObjectResult(new {error = $"Unable to find the requested products due to {e}"})
            {
                StatusCode = 500
            };
        }
    }

    public async Task<ActionResult<Product>> GetProductById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var results = await _source.GetProductById(id, cancellationToken);
            return results;
        }
        catch (Exception e)
        {
            return new ObjectResult(new {error = $"Unable to find the requested product {id} due to {e}"})
            {
                StatusCode = 500
            };
        }
    }
}