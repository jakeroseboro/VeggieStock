namespace VeganAPI.Models.Users;

public class VerifiedUser : User
{
    public string Token { get; set; }
}