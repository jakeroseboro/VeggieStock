namespace VeganAPI.Models.Products;

public interface IProductQueryService
{
    public Task<Product> GetProducts(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default);
    
    public Task<Product> GetProductById(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default);
}