using Microsoft.AspNetCore.Mvc;

namespace VeganAPI.Models.Users;

public interface IMongoUserSink
{
    public Task<ActionResult<User>> CreateUser(NewUser newUser, CancellationToken cancellationToken = default);
}