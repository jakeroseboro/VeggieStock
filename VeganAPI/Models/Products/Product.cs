using MongoDB.Bson.Serialization.Attributes;
using VeganAPI.Models.Stores;

namespace VeganAPI.Models.Products;

public class Product
{
    [BsonId]
    public Guid Id { get; set; }
    
    public IList<Sighting> Sightings { get; set; }

    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public IList<string> Images { get; set; }
    
    public string CoverImage { get; set; }
    
    public IList<int> ZipCodes { get; set; }
    
    public DateTime LastSpotted { get; set; }
    
    public bool IsDeleted { get; set; }
}