
using Microsoft.EntityFrameworkCore;
using MyWebApplication.Dtos;
using RekrutacjaApp.Data;
using RekrutacjaApp.Entities;

namespace RekrutacjaApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> VerifyName(string firstName, string lastName)
        {
            bool pairExist = await _context.Users.AnyAsync(u => u.Name == firstName && u.Surname == lastName);

            if (pairExist) return false;

            return true;
        }

        public async Task<List<User>> SearchUsers(SearchQuery searchQuery)
        {
            IQueryable<User> users = _context.Users.AsQueryable();

            if (!String.IsNullOrEmpty(searchQuery.SearchString))
            {
                users = users.Where(s => s.Name!.Contains(searchQuery.SearchString));
            }
            return await users.AsNoTracking().ToListAsync();
        }

        public async Task<PagedResult<List<User>>> GetUsers(QueryParams queryParams)
        {
            var query =  _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                query = query.Where(u => u.Name.Contains(queryParams.Name) || u.Surname.Contains(queryParams.Surname!));
            }

            var totalCount = await query.CountAsync();
            var result = await query
                .Skip((queryParams.page - 1) * queryParams.pageSize)
                .Take(queryParams.pageSize)
                .ToListAsync();

            return new PagedResult<List<User>>
            {
                CurrentPage = queryParams.page,
                PageSize = queryParams.pageSize,
                TotalCount = totalCount,
                Results = result
            };
        }

        public async Task<User> GetUser(Guid? id)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(i => i.Id == id);
            return user!;
        }
        public async Task<User> CreateUser(User createUserDto)
        {
            User newUser = new User()
            {
                Name = createUserDto.Name,
                Surname = createUserDto.Surname,
                BirthDate = createUserDto.BirthDate,
                Gender = createUserDto.Gender,             
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<bool> DeleteUser(Guid? userId)
        {
            User? userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userToDelete is null) throw new BadHttpRequestException("User not found");
            return true;
        }


        public async Task<User> UpdateUser(User updateUserDto, Guid? userId)
        {
            User? currentUser = await _context.Users.SingleAsync(r => r.Id == userId);

            if (currentUser is null) throw new BadHttpRequestException("Bad");

            currentUser.Name = updateUserDto.Name;
            currentUser.Surname = updateUserDto.Surname;
            currentUser.BirthDate = updateUserDto.BirthDate;
            currentUser.Gender = updateUserDto.Gender;

            _context.SaveChanges();
            return currentUser;
        }

        public async Task<List<User>> GetUsersForRaport()
        {
            List<User> allUsersForRaport = await _context.Users.ToListAsync();

            return allUsersForRaport;
        }
    }
}
