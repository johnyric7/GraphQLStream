using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLStream.DTO;

namespace GraphQLStream.Repositories
{
    public interface IMongoDbRepository
    {
        Task<IEnumerable<UserLocationDTO>> GetAllAsync();
        Task<UserLocationDTO> GetByUserIdAsync(Guid userId);
        Task<UserLocationDTO> CreateUserAsync(UserLocationDTO user);
        Task<UserLocationDTO> UpdateUserLocationAsync(UserLocationDTO user);
        Task<bool> DeleteAsync(Guid userId);
    }
}