namespace VeganAPI.Models.Products;

public interface IProductMongoSink
{
    public Task<Product> CreateProduct(Product product, CancellationToken cancellationToken = default);
    
    public Task<Product> UpdateProducts(Product product, CancellationToken cancellationToken = default);
}