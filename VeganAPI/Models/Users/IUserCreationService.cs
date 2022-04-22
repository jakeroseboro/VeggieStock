using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Users;

public interface IUserCreationService
{
    public Task<ActionResult<VerifiedUser>> CreateUser(NewUser newUser, CancellationToken cancellationToken = default);
}