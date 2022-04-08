namespace VeganAPI.Models.Products;

public static class ProductConfigurationExtension
{
     public static void AddProductServices(this IServiceCollection serviceCollection)
     {
         serviceCollection.AddSingleton<IProductMongoSink, MongoProductSink>();
         serviceCollection.AddSingleton<IMongoProductSource, MongoProductSource>();
         serviceCollection.AddSingleton<IProductQueryService, ProductQueryService>();
         serviceCollection.AddSingleton<IProductCreationService, ProductCreationService>();
         serviceCollection.AddSingleton<IProductUpdateService, ProductUpdateService>();
     }
}