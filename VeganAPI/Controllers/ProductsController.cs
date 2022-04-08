using Microsoft.AspNetCore.Mvc;
using VeganAPI.Models.Products;

namespace VeganAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductCreationService _creationService;
    private readonly IProductUpdateService _productUpdateService;
    private readonly IProductQueryService _queryService;

    public ProductsController(IProductCreationService creationService, IProductUpdateService productUpdateService, IProductQueryService queryService)
    {
        _creationService = creationService;
        _productUpdateService = productUpdateService;
        _queryService = queryService;
    }

    [HttpGet] 
    public async Task<ActionResult<IList<Product>>> GetProducts([FromQuery] ProductQueryOptions queryOptions)
    {
        var result = await _queryService.GetProducts(queryOptions, CancellationToken.None);
        return result;
    }
    
    [HttpGet("{productId:Guid}")] 
    public async Task<ActionResult<Product>> GetProductById([FromRoute] Guid productId)
    {
        var result = await _queryService.GetProductById(productId, CancellationToken.None);
        return result;
    }
}