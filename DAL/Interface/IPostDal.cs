using DTO;
using MongoDB.Bson;

namespace DAL.Interface
{
    public interface IPostDal
    {
        public bool Add(Posts post);
        public bool Delete();
        public bool AddComment(Comment comment, ObjectId postId);
        public bool LikeComment(Comment comment, ObjectId userId);
        public bool AddLike(ObjectId postId);
        public int CountLikes(ObjectId postId);
        public List<Posts> GetAll();
    }
}
