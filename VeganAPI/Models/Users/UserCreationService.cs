using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace VeganAPI.Models.Users;

public class UserCreationService: IUserCreationService
{
    private readonly IMongoUserSink _sink;
    private readonly IConfiguration _configuration;

    public UserCreationService(IMongoUserSink sink, IConfiguration configuration)
    {
        _sink = sink;
        _configuration = configuration;
    }

    public async Task<ActionResult<VerifiedUser>> CreateUser(NewUser newUser, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _sink.CreateUser(newUser, cancellationToken);
            var user = result.Value;
            
            if (user?.UserName == null)
                return new ObjectResult(new {error = "User not found"})
                {
                    StatusCode = 500
                };
            var token = GenerateJwtToken(user.UserName);
            return user.ToVerifiedUser(token);

        }
        catch (Exception e)
        {
            return new ObjectResult(new {error = $"Unable to create user due to {e}"})
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