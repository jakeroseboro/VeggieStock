namespace VeganAPI.Models.Users;

public static class UserConfigurationExtension
{
    public static void AddUserServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMongoUserSink, MongoUserSink>();
        serviceCollection.AddSingleton<IMongoUserSource, MongoUserSource>();
        serviceCollection.AddSingleton<IUserQueryService, UserQueryService>();
        serviceCollection.AddSingleton<IUserCreationService, UserCreationService>();
    }
}