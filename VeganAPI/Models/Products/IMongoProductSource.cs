namespace VeganAPI.Models.Products;

public interface IMongoProductSource
{
    public Task<IList<Product>> GetProducts(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default);
    
    public Task<Product> GetProductById(ProductQueryOptions queryOptions, CancellationToken cancellationToken = default);
}