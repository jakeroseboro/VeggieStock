using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Products;

public interface IProductCreationService
{
    public Task<ActionResult<Product>> CreateProduct(NewProduct newProduct, CancellationToken cancellationToken = default);
}