using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VeganAPI.Configuration;
using VeganAPI.Models.Products;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Setup CORS
var CorsPolicy = "CORS";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy,
        policy  =>
        {
            policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true);
        });
});

//Configure Database Settings
builder.Services.Configure<MongoDbConnectionSettings>(
    builder.Configuration.GetSection(nameof(MongoDbConnectionSettings)));
builder.Services.AddSingleton<IMongoDbConnectionSettings>(provider =>
    provider.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value);

//Configure product services
builder.Services.AddProductServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
