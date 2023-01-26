
using MyWebApplication.Dtos;
using RekrutacjaApp.Commands;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Queries;

namespace RekrutacjaApp.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUser(GetUserQuery query);      
        public Task<List<User>> GetUsers(GetUsersQuery query);
        public Task<Task> CreateUser(CreateUserCommand command);
        public Task<Task> UpdateUser(UpdateUserCommand command);
        public Task<Task> DeleteUser(DeleteUserCommand command);
        
        public Task<List<User>> GetUsersForRaport();
        public Task<bool> VerifyName(string firstName, string lastName);
    }
}
