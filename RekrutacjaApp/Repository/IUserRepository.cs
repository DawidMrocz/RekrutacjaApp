
using MyWebApplication.Dtos;
using RekrutacjaApp.Entities;

namespace RekrutacjaApp.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUser(Guid? userId);
        public Task<List<User>> GetUsersForRaport();
        public Task<PagedResult<List<User>>> GetUsers(QueryParams queryParams);
        public Task<List<User>> SearchUsers(SearchQuery searchQuery);
        public Task<User> CreateUser(User createUserDto);
        public Task<bool> DeleteUser(Guid? userId);
        public Task<User> UpdateUser(User updateUser, Guid? userId);

        public Task<bool> VerifyName(string firstName, string lastName);
    }
}
