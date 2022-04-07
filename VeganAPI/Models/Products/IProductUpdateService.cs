namespace VeganAPI.Models.Products;

public interface IProductUpdateService
{
    public Task<Product> UpdateProducts(ProductUpdateOptions updateOptions, CancellationToken cancellationToken = default);
}