namespace VeganAPI.Configuration;

public interface IMongoDbConnectionSettings
{
    string ProductsCollectionName { get; set; }
    string UsersCollectionName { get; set; }
    string StoresCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}