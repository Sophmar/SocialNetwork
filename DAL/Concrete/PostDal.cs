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
    public class PostDal : IPostDal
    {
        private const string ConnectionString = "mongodb+srv://khashchevskasophiamaria:12345@cluster0.qevoc.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

        private static readonly MongoClient _client = new(ConnectionString);

        private static readonly IMongoDatabase Database = _client.GetDatabase("social_network");


        public bool Add(Posts post)
        {
            var collection = Database.GetCollection<Posts>("posts");
            collection.InsertOne(post);
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
            var filter = Builders<Users>.Filter.Eq("Email", email);
            return collection.Find(filter).FirstOrDefault();
        }


        public bool Delete(string email)
        {
            var collection = Database.GetCollection<Users>("users");

            var filter = Builders<Users>.Filter.Eq("Email", email);
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
        
        public bool Add(Posts post)
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool AddComment(Comment comment, ObjectId postId)
        {
            throw new NotImplementedException();
        }

        public bool LikeComment(Comment comment, ObjectId userId, ObjectId postId)
        {
            var collection = Database.GetCollection<Posts>("posts");
            var filter = Builders<Posts>.Filter.Eq("_id", postId);
            Posts post = collection.Find(filter).FirstOrDefault();

            var updateDefinition2 = Builders<Posts>.Update.Push(u => u.Comments[].Likes, userId);
            var res2 = collection.UpdateOne(x => (x.UserId == comment.UserId && x.CreatedAt == comment.CreatedAt), updateDefinition2);

            if (res2.ModifiedCount > 0)
            {
                return true;
            }
            return false;
        }

        public bool AddLike(ObjectId postId, ObjectId userId)
        {
            var collectionPosts = Database.GetCollection<Posts>("posts");
            var updateDefinition2 = Builders<Posts>.Update.Push("likes", userId);
            var res = collectionPosts.UpdateOne(x => x.Id == postId, updateDefinition2);
            if (res.ModifiedCount > 0)
                return true;
            return false;
        }

        public int CountLikes(ObjectId postId)
        {
            var collection = Database.GetCollection<Posts>("posts");
            var filter = Builders<Posts>.Filter.Eq("_id", postId);
            var post = collection.Find(filter).FirstOrDefault();
            return post.Likes.Count();
        }

        public List<Posts> GetAll()
        {
            var collection = Database.GetCollection<Posts>("posts");
            var builder = Builders<Posts>.Filter;
            var filter = builder.Empty;
            var posts = collection.Find(filter).ToList();
            return posts;
        }
    }
}
