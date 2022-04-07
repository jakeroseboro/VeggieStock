namespace VeganAPI.Models.Products;

public interface IProductCreationService
{
    public Task<Product> CreateProduct(NewProduct newProduct, CancellationToken cancellationToken = default);
}