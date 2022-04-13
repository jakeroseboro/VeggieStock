using MongoDB.Bson.Serialization.Attributes;
using VeganAPI.Models.Products.Enums;
using VeganAPI.Models.Products.Subclasses;

namespace VeganAPI.Models.Products;

public class Product
{
    [BsonId]
    public Guid Id { get; set; }
    
    public IList<Sighting> Sightings { get; set; }

    public string Name { get; set; }
    
    public string Brand { get; set; }
    
    
    public ProductType Type { get; set; }
    
    
    public string CreatedBy { get; set; }
    
    public string CoverImage { get; set; }
    
    public IList<int> ZipCodes { get; set; }
    
    public DateTime LastSpotted { get; set; }
    
    public bool IsDeleted { get; set; }
}