using VeganAPI.Models.Stores;

namespace VeganAPI.Models.Products;

public class Sighting
{
    public Store Store { get; set; }
    
    public int ZipCode { get; set; }
}