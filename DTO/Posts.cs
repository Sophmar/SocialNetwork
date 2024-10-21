using MongoDB.Bson;

namespace DTO
{
    public class Posts
    {
        public ObjectId Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<int> Likes { get; set; }
        public List<Comment> Comments { get; set; }
    }
    public class Comment
    {
        public List<int> Likes { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
