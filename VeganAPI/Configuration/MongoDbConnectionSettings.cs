namespace VeganAPI.Configuration;

public class MongoDbConnectionSettings : IMongoDbConnectionSettings
{
    public string ProductsCollectionName { get; set; }
    
    public string UsersCollectionName { get; set; }
    
    public string StoresCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}