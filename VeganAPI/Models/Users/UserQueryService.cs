using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace VeganAPI.Models.Users;

public class UserQueryService: IUserQueryService
{
    private readonly IMongoUserSource _source;
    private readonly IConfiguration _configuration;

    public UserQueryService(IConfiguration configuration, IMongoUserSource source)
    {
        _configuration = configuration;
        _source = source;
    }
    
    public async Task<ActionResult<VerifiedUser>> GetUser(UserQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _source.GetUser(queryOptions, cancellationToken);
            var user = result.Value;
            
            if (user?.Id == Guid.Empty)
            {
                return new ObjectResult(new {error = "User not found"})
                {
                    StatusCode = 404
                };
            }

            if (user?.UserName == null)
            {
               return new ObjectResult(new {error = "User not found"})
               {
                   StatusCode = 404
               }; 
            }
                
            var token = GenerateJwtToken(user.UserName);
            return user.ToVerifiedUser(token);
        }
        catch (Exception e)
        {
            return new ObjectResult(new {error = $"Unable to find user due to {e}"})
            {
                StatusCode = 500
            };
        }
    }

    public async Task<ActionResult<IList<string>>> GetAllUsers(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _source.GetAllUsers(cancellationToken);
            var users = result.Value?.ToList();

            if (users == null)
            {
                return new ObjectResult(new {error = "User not found"})
                {
                    StatusCode = 404
                }; 
            }
            return users;
        }
        catch (Exception e)
        {
            return new ObjectResult(new {error = $"Unable to find user due to {e}"})
            {
                StatusCode = 500
            };
        }
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Role, "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

        var token = new JwtSecurityToken(
            null,
            null,
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}