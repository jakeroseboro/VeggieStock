using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeganAPI.Models.Products;

namespace VeganAPI.Controllers;

[Route("[controller]")]
[Authorize]
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
        try
        {
            var result = await _queryService.GetProducts(queryOptions, HttpContext.RequestAborted);
            return result;
        }
        catch (Exception e)
        {
            return new ObjectResult($"Unable to find products due to {e}")
            {
                StatusCode = 500
            };
        }
        
    }
    
    [HttpGet("{productId:Guid}", Name = nameof(GetProductById))] 
    public async Task<ActionResult<Product>> GetProductById([FromRoute] Guid productId)
    {
        try
        {
            var result = await _queryService.GetProductById(productId, HttpContext.RequestAborted);
            return result;
        }
        catch (Exception e)
        {
            return new ObjectResult($"Unable to find product due to {e}")
            {
                StatusCode = 500
            };
        }
        
    }
    
    [HttpPatch(Name = nameof(UpdateProduct))] 
    public async Task<ActionResult<Product>> UpdateProduct([FromBody] ProductUpdateOptions updateOptions)
    {
        try
        {
            updateOptions.Sighting.Seen = DateTime.Now;
            var result = await _productUpdateService.UpdateProduct(updateOptions, HttpContext.RequestAborted);
            return result;
        }
        catch (Exception e)
        {
            return new ObjectResult($"Unable to update product due to {e}")
            {
                StatusCode = 500
            };
        }
    }
    
    [HttpPost(Name = nameof(CreateProduct))] 
    public async Task<ActionResult<Product>> CreateProduct([FromBody] NewProduct newProduct)
    {
        try
        {
            newProduct.Sighting.Seen = DateTime.Now;
            var result = await _creationService.CreateProduct(newProduct, HttpContext.RequestAborted);
            return result;
        }
        catch (Exception e)
        {
            return new ObjectResult($"Unable to create product due to {e}")
            {
                StatusCode = 500
            };
        }
    }
}