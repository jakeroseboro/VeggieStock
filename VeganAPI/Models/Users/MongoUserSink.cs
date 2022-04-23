using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VeganAPI.Configuration;

namespace VeganAPI.Models.Users;

public class MongoUserSink : IMongoUserSink
{
    private readonly IMongoCollection<User> _users;
    public MongoUserSink(IMongoDbConnectionSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _users = database.GetCollection<User>(settings.UsersCollectionName);

        var indexBuilder = Builders<User>.IndexKeys;
        var indexModel = new CreateIndexModel<User>(indexBuilder
            .Ascending(x => x.UserName),
            new CreateIndexOptions{ Unique = true }
        );
        _users.Indexes.CreateOneAsync(indexModel, cancellationToken: CancellationToken.None);
    }
    
    public async Task<ActionResult<User>> CreateUser(NewUser newUser, CancellationToken cancellationToken = default)
    {
        var crypto = new UserCrypto();
        var pass = EncryptPassword(newUser.Password, crypto.Salt);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Password = pass,
            Email = newUser.Email,
            UserName = newUser.UserName
        };
        await _users.InsertOneAsync(user, new InsertOneOptions(), cancellationToken);
        return user;
    }

    private string EncryptPassword(string password, string salt)
    {
        var guidSalt = new Guid(salt).ToByteArray();
        byte[] iv = guidSalt;
        byte[] key = guidSalt;
        byte[] array;  
        
        using (Aes aes = Aes.Create())  
        {  
            aes.Key = key;  
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;
  
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);  
  
            using (MemoryStream memoryStream = new MemoryStream())  
            {  
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))  
                {  
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))  
                    {  
                        streamWriter.Write(password);  
                    }  
  
                    array = memoryStream.ToArray();  
                }  
            }  
        }  
  
        return Convert.ToBase64String(array);  
    }
}