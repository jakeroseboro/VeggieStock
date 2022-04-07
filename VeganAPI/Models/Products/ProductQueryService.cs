using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Products;

public class ProductQueryService : IProductQueryService
{
    public Task<ActionResult<IList<Product>>> GetProducts(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<Product>> GetProductById(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}