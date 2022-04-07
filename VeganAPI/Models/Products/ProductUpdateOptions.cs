using VeganAPI.Models.Stores;

namespace VeganAPI.Models.Products;

public class ProductUpdateOptions
{
    public IList<Sighting>? Sightings { get; set; }
}