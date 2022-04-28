using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeganAPI.Models.Users;

namespace VeganAPI.Controllers;

[Route("[controller]")]
[AllowAnonymous]
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
    
    [HttpPost("Login",Name = nameof(Login))] 
    public async Task<ActionResult<VerifiedUser>> Login([FromBody] UserQueryOptions queryOptions)
    {
        try
        {
            var result = await _queryService.GetUser(queryOptions, HttpContext.RequestAborted);
            return result;
        }
        catch (Exception e)
        {
            return new ObjectResult($"Unable to find user due to {e}")
            {
                StatusCode = 500
            };
        }
        
    }

    [HttpPost("CreateUser", Name = nameof(CreateUser))]
    public async Task<ActionResult<VerifiedUser>> CreateUser([FromBody] NewUser newUser)
    {
        try
        {
            var result = await _creationService.CreateUser(newUser, HttpContext.RequestAborted);
            return result;
        }
        catch (Exception e)
        {
            return new ObjectResult($"Unable to create user due to {e}")
            {
                StatusCode = 500
            };
        }
    }

    [HttpGet("GetAllUsers", Name = nameof(GetAllUsers))]
    public async Task<ActionResult<IList<string>>> GetAllUsers()
    {
        try
        {
            var result = await _queryService.GetAllUsers(HttpContext.RequestAborted);
            return result;
        }
        catch (Exception e)
        {
            return new ObjectResult($"Unable to find users due to {e}")
            {
                StatusCode = 404
            };
        }
    }
}