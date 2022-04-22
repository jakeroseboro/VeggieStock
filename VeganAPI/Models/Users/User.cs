using MongoDB.Bson.Serialization.Attributes;

namespace VeganAPI.Models.Users;

public class User
{
    [BsonId]
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
}