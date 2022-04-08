using VeganAPI.Models.Stores;

namespace VeganAPI.Models.Products;

public class ProductUpdateOptions
{
    public Guid Id { get; set; }
    public IList<Sighting>? Sightings { get; set; }
}