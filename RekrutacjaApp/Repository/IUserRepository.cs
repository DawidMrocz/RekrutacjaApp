
using MyWebApplication.Dtos;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;

namespace RekrutacjaApp.Repositories
{
    public interface IUserRepository
    {
        public Task<UserDto> GetUser(int? userId);      
        public Task<PagedResult<List<UserDto>>> GetUsers(QueryParams queryParams);

        public Task<User> CreateUser(User createUserDto);
        public Task<User> UpdateUser(User updateUser, int? userId);
        public Task<bool> DeleteUser(int? userId);
        

        public Task<List<User>> GetUsersForRaport();
        public Task<bool> VerifyName(string firstName, string lastName);
    }
}
