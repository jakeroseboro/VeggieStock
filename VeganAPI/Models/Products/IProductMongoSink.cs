using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Products;

public interface IProductMongoSink
{
    public Task<ActionResult<Product>> CreateProduct(Product product, CancellationToken cancellationToken = default);
    
    public Task<ActionResult<Product>> UpdateProduct(Product product, ProductUpdateOptions updateOptions, CancellationToken cancellationToken = default);

}