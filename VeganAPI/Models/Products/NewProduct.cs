using VeganAPI.Models.Stores;

namespace VeganAPI.Models.Products;

public class NewProduct
{
    public string Name { get; set; }
    
    public Sighting Sighting { get; set; }

    public string Description { get; set; }
    
    public IList<string> Images { get; set; }
    
    public string CoverImage { get; set; }
}