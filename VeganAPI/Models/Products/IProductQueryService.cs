using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Products;

public interface IProductQueryService
{
    public Task<ActionResult<IList<Product>>> GetProducts(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default);
    
    public Task<ActionResult<Product>> GetProductById(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default);
}