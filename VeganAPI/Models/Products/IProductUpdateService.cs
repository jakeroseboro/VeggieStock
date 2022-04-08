using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Products;

public interface IProductUpdateService
{
    public Task<ActionResult<Product>> UpdateProduct(ProductUpdateOptions updateOptions, CancellationToken cancellationToken = default);
}
