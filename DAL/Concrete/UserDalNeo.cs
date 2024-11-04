using DTO;
using Neo4jClient;
using System.Collections.Generic;
using System.Threading.Tasks;

public class User
{
    public int Id { get; set; }

}
    public class UserDalNeo
    {
        private readonly IGraphClient _client = new GraphClient(new Uri("bolt://localhost:7687/"), "SN Neo4j", "12345678");
        
    public UserDalNeo()
    {
            _client.ConnectAsync().Wait();
    }

    public async Task<bool> CreateUserAsync(int userId)
        {
        try
        {
            var user = new User { Id = userId };
            await _client.Cypher
                 .Create("(nu:User $newUser)")
                 .WithParam("newUser", user)
                 .ExecuteWithoutResultsAsync();
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
    public async Task<bool> DeleteUserAsync(int userId)
    {
        try
        {
            await _client.Cypher
                .Match("(u:User {id: $id})")
                .WithParam("id", userId)
                .DetachDelete("u")
                .ExecuteWithoutResultsAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }




    public async Task FollowUserAsync(string followerId, string followedId)
        {
            await _client.Cypher
                    .Match("(f:User {id: $fid})", "(u:User {id: $uid})")
                    .WithParam("fid", followerId)
                    .WithParam("uid", followedId)
                    .Create("(f)-[FOLLOWS]->(u)")
                    .ExecuteWithoutResultsAsync();
        }

        public async Task UnfollowUserAsync(string followerId, string followedId)
        {
            await _client.Cypher
                      .Match("(f:User {id: $fid})-[r:FOLLOWS]->(u:User {id: $uid})")
                      .WithParam("fid", followerId)
                      .WithParam("uid", followedId)
                      .Delete("r")
                      .ExecuteWithoutResultsAsync();
        }

        public async Task<List<User>> GetFollowersAsync(string userId)
        {
            var result = await _client.Cypher
                          .Match("(u:User {id: $id})<-[:FOLLOWS]-(f:User)")
                          .WithParam("id", userId)
                          .Return(f => f.As<User>())
                          .ResultsAsync;
            return result.ToList();
        }

        public async Task<List<User>> GetFollowingAsync(string userId)
        {
            var result = await _client.Cypher
                              .Match("(u:User {id: $id})-[:FOLLOWS]->(f:User)")
                              .WithParam("id", userId)
                              .Return(f => f.As<User>())
                              .ResultsAsync;
            return result.ToList();

        }
        public async Task<int?> GetDistanceBetweenUsersAsync(string userId1, string userId2)
        {
            var result = await _client.Cypher
                              .Match("(u1:User {id: $id1}), (u2:User {id: $id2})")
                              .Match("p = shortestPath((u1)-[:FOLLOWS*]->(u2))")
                              .WithParam("id1", userId1)
                              .WithParam("id2", userId2)
                              .Return<int?>("length(p)")
                              .ResultsAsync;
            return result.FirstOrDefault();
        }

    }
