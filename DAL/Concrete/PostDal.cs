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
            var result = collection.Find(p => p.Id == post.Id).FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            return false;
        }


        public bool Delete(ObjectId id)
        {
            var collection = Database.GetCollection<Posts>("posts");

            var filter = Builders<Posts>.Filter.Eq("_id", id);
            var result = collection.DeleteOne(filter);

            return (result.DeletedCount > 0);
        }


        public bool AddComment(ObjectId postId, Comment comment)
        {
            var collectionPosts = Database.GetCollection<Posts>("posts");
            var updateDefinition2 = Builders<Posts>.Update.Push("comments", comment);
            var res = collectionPosts.UpdateOne(x => x.Id == postId, updateDefinition2);
            return (res.ModifiedCount > 0) ;
        }

        public async Task<bool> LikeComment(ObjectId postId, ObjectId commentId, ObjectId userId)
        {
            var collection = Database.GetCollection<Posts>("posts");
            var builder = Builders<Posts>.Filter;
            var filter = Builders<Posts>.Filter.And(
            Builders<Posts>.Filter.Eq(post => post.Id, postId),
            Builders<Posts>.Filter.ElemMatch(post => post.Comments, comment => comment.Id == commentId));
            var update = Builders<Posts>.Update.AddToSet("comments.$.like", userId);

            var updateResult = await collection.UpdateOneAsync(filter, update);

            return (updateResult.ModifiedCount > 0);
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
