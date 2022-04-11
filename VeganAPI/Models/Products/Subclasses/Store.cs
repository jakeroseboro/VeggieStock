using MongoDB.Bson.Serialization.Attributes;

namespace VeganAPI.Models.Products.Subclasses;

public class Store
{
    [BsonId]
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public string Logo { get; set; }
}