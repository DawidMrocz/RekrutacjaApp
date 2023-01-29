
using MyWebApplication.Dtos;
using RekrutacjaApp.Commands;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Queries;


namespace RekrutacjaApp.Repositories
{
    //public interface IUserRepository: IGenericRepository<User>
    //{
    //    public Task<User> GetUser(GetUserQuery query);      
    //    public Task<List<User>> GetUsers(GetUsersQuery query);
    //    public Task<Task> CreateUser(CreateUserCommand command);
    //    public Task<Task> UpdateUser(UpdateUserCommand command);
    //    public Task<Task> DeleteUser(DeleteUserCommand command);
    //    public Task<User> AddAttribute(CustomAttributeDto command, int userId);
    //    public Task<Task> RemoveAttribute(int attributeId);
    //    public Task<List<User>> GetUsersForRaport();
    //    public Task<bool> VerifyName(string firstName, string lastName);
    //}

    public interface IUserRepository
    {
        public Task<UserDto> GetUser(GetUserQuery query);
        public Task<List<UserDto>> GetUsers(GetUsersQuery query);
        public Task<bool> CreateUser(CreateUserCommand command);
        public Task<bool> UpdateUser(UpdateUserCommand command);
        public Task<bool> DeleteUser(DeleteUserCommand command);
        public Task<User> AddAttribute(CustomAttributeDto command, int userId);
        public Task<bool> RemoveAttribute(RemoveAttributeCommand command);
        public Task<List<User>> GetUsersForRaport();
        public bool VerifyName(string firstName, string lastName);
    }
}
