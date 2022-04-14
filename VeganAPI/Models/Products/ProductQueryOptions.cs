namespace VeganAPI.Models.Products;

public class ProductQueryOptions
{
    public string? Name { get; set; }
    
    public string? StoreName { get; set; }

    public IList<int>? ZipCodes { get; set; }
    
    public string? CreatedBy { get; set; }
}