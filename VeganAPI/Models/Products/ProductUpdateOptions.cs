using VeganAPI.Models.Products.Subclasses;

namespace VeganAPI.Models.Products;

public class ProductUpdateOptions
{
    public Guid Id { get; set; }
    public Sighting Sighting { get; set; }
    
    public bool? IsDeleted { get; set; }
}