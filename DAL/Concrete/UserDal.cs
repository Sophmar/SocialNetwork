using DAL.Interface;
using DTO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL.Concrete
{
    public class UserDal : IUserDal
    {
        private const string ConnectionString = "mongodb+srv://khashchevskasophiamaria:12345@cluster0.qevoc.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

        private static readonly MongoClient _client = new(ConnectionString);

        private static readonly IMongoDatabase Database = _client.GetDatabase("social_network");


        public bool Add(Users user)
        {
            var collection = Database.GetCollection<Users>("users");
            collection.InsertOne(user);
            var result = collection.Find(u => u.Email == user.Email).FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            return false;
        }


        public Users GetByEmail(string email)
        {
            var collection = Database.GetCollection<Users>("users");
            var filter = Builders<Users>.Filter.Eq("email", email);
            return collection.Find(filter).FirstOrDefault();
        }


        public bool Delete(string email)
        {
            var collection = Database.GetCollection<Users>("users");

            var filter = Builders<Users>.Filter.Eq("email", email);
            var result = collection.DeleteOne(filter);

            if (result.DeletedCount > 0)
            {
                return true;
            }
            return false;
        }


        public List<Users> GetAll()
        {
            var collection = Database.GetCollection<Users>("users");
            var builder = Builders<Users>.Filter;
            var filter = builder.Empty;
            var users = collection.Find(filter).ToList();
            return users;
        }

        public bool Update(Users user)
        {
            var collection = Database.GetCollection<Users>("users");
            var updateDefinition2 = Builders<Users>.Update.Set(u => u.Password, user.Password);
            var res2 = collection.UpdateOne(x => x.Email == user.Email, updateDefinition2);
            var updateDefinition3 = Builders<Users>.Update.Set(u => u.FirstName, user.FirstName);
            var res3 = collection.UpdateOne(x => x.Email == user.Email, updateDefinition3);
            var updateDefinition4 = Builders<Users>.Update.Set(u => u.LastName, user.LastName);
            var res4 = collection.UpdateOne(x => x.Email == user.Email, updateDefinition4);

            if (res2.ModifiedCount > 0 || res3.ModifiedCount > 0 || res4.ModifiedCount > 0)
            {
                return true;
            }
            return false;
        }
        public bool CheckPasswd(string email, string passwd)
        {
            var collection = Database.GetCollection<Users>("users");
            var user = collection.Find(u => u.Email == email).FirstOrDefault();

            if (user == null)
                return false;
            string hashedPassword = HashPassword(passwd);
            if (user.Password == hashedPassword)
                return true;
            return false;
        }
        public string HashPassword(string passwd)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
