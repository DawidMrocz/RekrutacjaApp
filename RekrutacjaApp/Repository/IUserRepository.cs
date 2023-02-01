
using MyWebApplication.Dtos;
using RekrutacjaApp.Commands;
using RekrutacjaApp.Data;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Queries;


namespace RekrutacjaApp.Repositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        //public Task<UserDto> GetUser(GetUserQuery query);
        //public Task<List<UserDto>> GetUsers(GetUsersQuery query);
        //public Task<bool> CreateUser(CreateUserCommand command);
        //public Task<bool> UpdateUser(UpdateUserCommand command);
        //public Task<bool> DeleteUser(DeleteUserCommand command);
        public Task<User> AddAttribute(CustomAttributeDto command, int userId);
        public Task<bool> RemoveAttribute(RemoveAttributeCommand command);
        public Task<List<User>> GetUsersForRaport();
        public bool VerifyName(string firstName, string lastName);
    }
}
