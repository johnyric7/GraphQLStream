using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLStream.DTO;
using MongoDB.Driver;

namespace GraphQLStream.Repositories
{
    public class MongoDbRepository : IMongoDbRepository
    {
        private readonly IMongoCollection<UserLocationDTO> _userCollection;

        public MongoDbRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("UserLocationDb");  // MongoDB database name
            _userCollection = database.GetCollection<UserLocationDTO>("UserLocations");  // Collection name
        }

        public async Task<IEnumerable<UserLocationDTO>> GetAllAsync()
        {
            return await _userCollection.Find(userLocation => true).ToListAsync();
        }

        public async Task<UserLocationDTO> GetByUserIdAsync(Guid userId)
        {
            return await _userCollection
                .Find(user => user.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<UserLocationDTO> CreateUserAsync(UserLocationDTO user)
        {
            await _userCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<UserLocationDTO> UpdateUserLocationAsync(UserLocationDTO user)
        {
            var result = await _userCollection.ReplaceOneAsync(
                c => c.UserId == user.UserId,
                user);

            // Return the updated user if found, otherwise null
            return result.MatchedCount > 0 ? user : null;
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var result = await _userCollection.DeleteOneAsync(user => user.UserId == userId);
            return result.DeletedCount > 0;
        }
    }

}