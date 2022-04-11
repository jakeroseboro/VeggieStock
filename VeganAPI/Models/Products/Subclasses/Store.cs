using MongoDB.Bson.Serialization.Attributes;

namespace VeganAPI.Models.Products.Subclasses;

public class Store
{
    public string Name { get; set; }

    public string? Logo { get; set; }
}