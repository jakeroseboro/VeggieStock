using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Users;

public interface IMongoUserSource
{
    public Task<ActionResult<User>> GetUser(UserQueryOptions queryOptions, CancellationToken cancellationToken = default);

    public Task<ActionResult<IList<string>>> GetAllUsers(CancellationToken cancellationToken = default);
}