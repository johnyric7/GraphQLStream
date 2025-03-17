using GraphQLStream.DTO;
using GraphQLStream.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQLStream.Schema.Queries
{
    public class Query
    {
        private readonly IMongoDbRepository _mongoDbRepository;

        public Query(MongoDbRepository mongoDbRepository)
        {
            _mongoDbRepository = mongoDbRepository;
        }

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 100)]
        public async Task<IEnumerable<UserLocationType>> GetUsers()
        {
            IEnumerable<UserLocationDTO> userLocationDTO = await _mongoDbRepository.GetAllAsync();

            IEnumerable<UserLocationType> users = userLocationDTO
                .Select(u => new UserLocationType()
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Latitude = u.Latitude,
                    Longitude = u.Longitude,
                    CreatedTimestamp = u.CreatedTimestamp,
                    UpdatedTimestamp = u.UpdatedTimestamp
                });

            return users;
        }

        public async Task<UserLocationType> GetUserByUserIdAsync(Guid id)
        {
            await Task.Delay(1000);

            UserLocationDTO userLocationDTO = await _mongoDbRepository.GetByUserIdAsync(id);

            UserLocationType userLocation = new UserLocationType()
            {
                UserId = userLocationDTO.UserId,
                Name = userLocationDTO.Name,
                Latitude = userLocationDTO.Latitude,
                Longitude = userLocationDTO.Longitude,
                CreatedTimestamp = userLocationDTO.CreatedTimestamp,
                UpdatedTimestamp = userLocationDTO.UpdatedTimestamp
            };

            return userLocation;
        }
    }
}