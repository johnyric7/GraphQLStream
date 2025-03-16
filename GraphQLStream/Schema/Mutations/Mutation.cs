using GraphQLStream.DTO;
using GraphQLStream.Repositories;
using HotChocolate;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace GraphQLStream.Schema.Mutations
{
    public class Mutation
    {
        private readonly IMongoDbRepository _mongoDbRepository;

        public Mutation(IMongoDbRepository mongoDbRepository)
        {
            _mongoDbRepository = mongoDbRepository;
        }

        public async Task<UserLocationResult> CreateUser(UserLocationTypeInput userInput, [Service] IConnectionMultiplexer publisher)
        {
            UserLocationDTO userDTO = new UserLocationDTO()
            {
                Name = userInput.Name,
                Latitude = userInput.Latitude,
                Longitude = userInput.Longitude,
                CreatedTimestamp = DateTime.UtcNow
            };

            userDTO = await _mongoDbRepository.CreateUserAsync(userDTO);

            UserLocationResult user = new UserLocationResult()
            {
                UserId = userDTO.UserId,
                Name = userDTO.Name,
                Latitude = userDTO.Latitude,
                Longitude = userDTO.Longitude,
                CreatedTimestamp = userDTO.CreatedTimestamp
            };

            string userJson = JsonSerializer.Serialize(user);
            await publisher.GetSubscriber().PublishAsync("UserCreated", userJson);

            return user;
        }

        public async Task<UserLocationResult> UpdateUserLocation(Guid userId, UserLocationTypeInput userInput, [Service] IConnectionMultiplexer publisher)
        {
            UserLocationDTO userLocationDTO = await _mongoDbRepository.GetByUserIdAsync(userId);

            if (userLocationDTO == null)
            {
                throw new GraphQLException(new Error("User not found.", "USER_NOT_FOUND"));
            }

            userLocationDTO.Name = userInput.Name;
            userLocationDTO.Latitude = userInput.Latitude;
            userLocationDTO.Longitude = userInput.Longitude;
            userLocationDTO.UpdatedTimestamp = DateTime.UtcNow;

            userLocationDTO = await _mongoDbRepository.UpdateUserLocationAsync(userLocationDTO);

            UserLocationResult user = new UserLocationResult()
            {
                UserId = userLocationDTO.UserId,
                Name = userLocationDTO.Name,
                Latitude = userLocationDTO.Latitude,
                Longitude = userLocationDTO.Longitude,
                CreatedTimestamp = userLocationDTO.CreatedTimestamp,
                UpdatedTimestamp = userLocationDTO.UpdatedTimestamp,
            };

            string updateUserLocationTopic = $"{user.UserId}_UserLocationUpdated";
            string userJson = JsonSerializer.Serialize(user);
            await publisher.GetSubscriber().PublishAsync(updateUserLocationTopic, userJson);

            return user;
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            try
            {
                return await _mongoDbRepository.DeleteAsync(userId);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}