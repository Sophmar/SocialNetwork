using MongoDB.Bson;

namespace DTO
{
    public class Users
    {
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Interests { get; set; }
        public List<int> PostsId { get; set; }
        public List<int> FollowersId { get; set; }
        public List<int> FollowingId { get; set; }
    }
}
