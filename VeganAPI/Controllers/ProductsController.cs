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

    [HttpGet(Name = nameof(GetProducts))] 
    public async Task<ActionResult<IList<Product>>> GetProducts([FromQuery] ProductQueryOptions queryOptions)
    {
        var result = await _queryService.GetProducts(queryOptions, HttpContext.RequestAborted);
        return result;
    }
    
    [HttpGet("{productId:Guid}", Name = nameof(GetProductById))] 
    public async Task<ActionResult<Product>> GetProductById([FromRoute] Guid productId)
    {
        var result = await _queryService.GetProductById(productId, HttpContext.RequestAborted);
        return result;
    }
    
    [HttpPatch(Name = nameof(UpdateProduct))] 
    public async Task<ActionResult<Product>> UpdateProduct([FromBody] ProductUpdateOptions updateOptions)
    {
        var result = await _productUpdateService.UpdateProduct(updateOptions, HttpContext.RequestAborted);
        return result;
    }
    
    [HttpPost(Name = nameof(CreateProduct))] 
    public async Task<ActionResult<Product>> CreateProduct([FromBody] NewProduct newProduct)
    {
        newProduct.Sighting.Seen = DateTime.Now;
        var result = await _creationService.CreateProduct(newProduct, HttpContext.RequestAborted);
        return result;
    }
}