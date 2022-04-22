using Microsoft.AspNetCore.Mvc;
using VeganAPI.Models.Users;

namespace VeganAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserCreationService _creationService;
    private readonly IUserQueryService _queryService;

    public UsersController(IUserCreationService creationService, IUserQueryService queryService)
    {
        _creationService = creationService;
        _queryService = queryService;
    }
    
    [HttpGet(Name = nameof(GetProducts))] 
    public async Task<ActionResult<VerifiedUser>> GetProducts([FromQuery] UserQueryOptions queryOptions)
    {
        try
        {
            var result = await _queryService.GetUser(queryOptions, HttpContext.RequestAborted);
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
}