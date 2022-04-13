using VeganAPI.Models.Products.Enums;
using VeganAPI.Models.Products.Subclasses;

namespace VeganAPI.Models.Products;

public class NewProduct
{
    public string Name { get; set; }
    
    public Sighting Sighting { get; set; }

    public string Brand { get; set; }

    public ProductType Type { get; set; }
    
    public string CreatedBy { get; set; }
    
    public string CoverImage { get; set; }
}