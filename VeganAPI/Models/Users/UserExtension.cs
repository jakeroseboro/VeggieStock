namespace VeganAPI.Models.Users;

public static class UserExtension
{
    public static VerifiedUser ToVerifiedUser(this User user, string token)
    {
        return new VerifiedUser
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Password = user.Password,
            Token = token
        };
    }
}