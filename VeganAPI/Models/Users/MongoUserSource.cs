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
        _users = database.GetCollection<User>(settings.UsersCollectionName);
    }
    public async Task<ActionResult<User>> GetUser(UserQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        var builder = Builders<User>.Filter;
        var filter = builder.Empty;

        var crypto = new UserCrypto();
        var encrypted = EncryptPassword(queryOptions.Password, crypto.Salt);
        filter &= builder.Eq(x => x.UserName, queryOptions.UserName);
        filter &= builder.Eq(x => x.Password, encrypted);

        var users = await _users.Find(filter).ToListAsync(cancellationToken);

        return users.FirstOrDefault() ?? new User{Id = Guid.Empty};
    }

    public async Task<ActionResult<IList<string>>> GetAllUsers(CancellationToken cancellationToken = default)
    {
        var builder = Builders<User>.Filter;
        var filter = builder.Empty;
        
        var users = await _users.Find(filter).ToListAsync(cancellationToken);

        var usernames = new List<string>();
        
        if (users.Count > 0)
        {
            foreach (var user in users)
            {
                usernames.Add(user.UserName);
            }
        }

        return usernames;
    }

    /// <summary>
    /// Decrypts encrypted password. Did not end up using this method but I wanted to leave it in anyways
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public string DecryptPassword(string password, string salt)
    {
        var guidSalt = new Guid(salt).ToByteArray();
        byte[] iv = guidSalt;
        byte[] key = guidSalt;
        byte[] buffer = Convert.FromBase64String(password);  
  
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;
            
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