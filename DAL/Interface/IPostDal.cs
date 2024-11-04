using DTO;
using MongoDB.Bson;

namespace DAL.Interface
{
    public interface IPostDal
    {
        public bool Add(Posts post);
        public bool Delete(ObjectId id);
        public bool AddComment(ObjectId postId, Comment comment);
        public Task<bool> LikeComment(ObjectId postId, ObjectId commentId, ObjectId userId);
        public bool AddLike(ObjectId postId, ObjectId userId);
        public int CountLikes(ObjectId postId);
        public List<Posts> GetAll();
    }
}
