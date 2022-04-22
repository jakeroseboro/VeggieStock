using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Users;

public interface IUserQueryService
{
    public Task<ActionResult<VerifiedUser>> GetUser(UserQueryOptions queryOptions, CancellationToken cancellationToken = default);
}