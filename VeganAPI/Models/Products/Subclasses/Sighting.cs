namespace VeganAPI.Models.Products.Subclasses;

public class Sighting
{
    public Store Store { get; set; }
    
    public string SpottedBy { get; set; }
    
    public DateTime Seen { get; set; }
    
    public string Street { get; set; }
    
    public string City { get; set; }
    
    public string State { get; set; }
    public int ZipCode { get; set; }
}