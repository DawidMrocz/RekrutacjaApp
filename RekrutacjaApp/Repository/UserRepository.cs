
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MyWebApplication.Dtos;
using Newtonsoft.Json;
using RekrutacjaApp.Commands;
using RekrutacjaApp.Data;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Helpers;
using RekrutacjaApp.Queries;

namespace RekrutacjaApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;      
        private readonly IDistributedCache _cache;

        public UserRepository(ApplicationDbContext context,IDistributedCache cache)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<bool> VerifyName(string firstName, string lastName)
        {
            bool pairExist = await _context.Users.AnyAsync(u => u.Name == firstName && u.Surname == lastName);

            if (pairExist) return false;

            return true;
        }

        public async Task<List<User>> GetUsers(GetUsersQuery command)
        {
            string key = JsonConvert.SerializeObject(command.queryParams);
            List<User>? users = await _cache.GetRecordAsync<List<User>>(key);
            if (users is null)
            {

                var query = _context.Users
                .Include(attr => attr.CustomAttributes)
                .AsQueryable();

                if (!string.IsNullOrEmpty(command.queryParams.SearchString))
                {
                    query = query.Where(u => u.Name.Contains(command.queryParams.SearchString) || u.Surname.Contains(command.queryParams.SearchString));
                };

                if (command.queryParams.AgeMin is not null)
                {
                    query = query.Where(u => u.Age >= command.queryParams.AgeMin);
                };

                if (command.queryParams.AgeMax is not null)
                {
                    query = query.Where(u => u.Age <= command.queryParams.AgeMax);
                };

                switch (command.queryParams.SortOrder)
                {
                    case "name":
                        query = query.OrderBy(u => u.Name); break;
                    case "surname":
                        query = query.OrderBy(s => s.Surname); break;
                    default:
                        break;
                }

                var totalCount = await query.CountAsync();
                users = await query
                    .Skip((command.queryParams.page - 1) * command.queryParams.pageSize)
                    .Take(command.queryParams.pageSize)
                    .AsNoTracking()
                    .ToListAsync();

                await _cache.SetRecordAsync(key, users);
            }         
            return users;
        }

        public async Task<User> GetUser(GetUserQuery command)
        {

            User? user = await _cache.GetRecordAsync<User>($"User_{command.userId}");
            if (user is null)
            {
                user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(i => i.UserId == command.userId);
                await _cache.SetRecordAsync($"User_{command.userId}", user);
            }

            if (user is null) throw new BadHttpRequestException("User not found");
            return user;
        }
        public async Task<Task> CreateUser(CreateUserCommand command)
        {
            User newUser = new User()
            {
                Name = command.user.Name,
                Surname = command.user.Surname,
                BirthDate = command.user.BirthDate,
                Gender = command.user.Gender,             
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<Task> DeleteUser(DeleteUserCommand command)
        {
            User? userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.UserId == command.UserId);
            if (userToDelete is null) throw new BadHttpRequestException("User not found");
            _context.Remove(userToDelete);
            await _cache.DeleteRecordAsync<User>($"User_{userToDelete.UserId}");
            return Task.CompletedTask;
        }

        public async Task<Task> UpdateUser(UpdateUserCommand command)
        {

            User? currentUser = await _context.Users.FirstOrDefaultAsync(r => r.UserId == command.user.UserId);
            if (currentUser is null) throw new BadHttpRequestException("Bad");

            currentUser.Name = command.user.Name;
            currentUser.Surname = command.user.Surname;
            currentUser.BirthDate = command.user.BirthDate;
            currentUser.Gender = command.user.Gender;
            await _context.SaveChangesAsync();
            await _cache.DeleteRecordAsync<UserDto>($"User_{command.user.UserId}");
            return Task.CompletedTask;
        }

        public async Task<List<User>> GetUsersForRaport()
        {
            List<User> allUsersForRaport = await _context.Users.ToListAsync();
            return allUsersForRaport;
        }
    }
}
