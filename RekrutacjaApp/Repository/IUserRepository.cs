
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
        public Task<User> AddAttribute(CustomAttributeDto command, int userId);
        public Task<bool> RemoveAttribute(RemoveAttributeCommand command);
        public Task<List<User>> GetUsersForRaport();
        public bool VerifyName(string firstName, string lastName);
    }
}
