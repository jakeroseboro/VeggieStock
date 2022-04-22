using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VeganAPI.Configuration;

namespace VeganAPI.Models.Users;

public class MongoUserSource: IMongoUserSource
{
    private readonly IMongoCollection<User> _users;
    public MongoUserSource(IMongoDbConnectionSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _users = database.GetCollection<User>(settings.ProductsCollectionName);
    }
    public async Task<ActionResult<User>> GetUser(UserQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        var builder = Builders<User>.Filter;
        var filter = builder.Empty;

        var crypto = new UserCrypto();
        var decrypted = DecryptPassword(queryOptions.Password, crypto.Salt);
        filter &= builder.Eq(x => x.UserName, decrypted);
        filter &= builder.Eq(x => x.Password, queryOptions.Password);

        var products = await _users.Find(filter).ToListAsync(cancellationToken);

        return products.FirstOrDefault() ?? new User{Id = Guid.Empty};
    }

    public string DecryptPassword(string password, string salt)
    {
        byte[] iv = Encoding.UTF8.GetBytes(salt);
        byte[] key = Encoding.UTF8.GetBytes(salt);
        byte[] buffer = Convert.FromBase64String(password);  
  
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;  
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);  
  
            using (MemoryStream memoryStream = new MemoryStream(buffer))  
            {  
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))  
                {  
                    using (StreamReader streamReader = new StreamReader(cryptoStream))  
                    {  
                        return streamReader.ReadToEnd();  
                    }  
                }  
            }  
        }
    }
}