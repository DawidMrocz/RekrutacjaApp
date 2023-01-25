
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyWebApplication.Dtos;
using Newtonsoft.Json;
using RekrutacjaApp.Data;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;

namespace RekrutacjaApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> VerifyName(string firstName, string lastName)
        {
            bool pairExist = await _context.Users.AnyAsync(u => u.Name == firstName && u.Surname == lastName);

            if (pairExist) return false;

            return true;
        }

        public async Task<List<UserDto>> GetUsers(QueryParams queryParams)
        {
            var query = _context.Users
                .Include(attr => attr.CustomAttributes)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.SearchString)) 
            {
                query = query.Where(u => u.Name.Contains(queryParams.SearchString) || u.Surname.Contains(queryParams.SearchString));
            };

            if (queryParams.AgeMin is not null)
            {
                query = query.Where(u => u.Age >= queryParams.AgeMin);
            };

            if (queryParams.AgeMax is not null)
            {
                query = query.Where(u => u.Age <= queryParams.AgeMax);
            };

            if (!string.IsNullOrEmpty(queryParams.SearchString))
            {
                query = query.Where(u => u.Name.Contains(queryParams.SearchString) || u.Surname.Contains(queryParams.SearchString));
            };

            var totalCount = await query.CountAsync();
            var result = await query
                .Skip((queryParams.page - 1) * queryParams.pageSize)
                .Take(queryParams.pageSize)
                .OrderBy(n => n.Name)
                .ThenBy(s => s.Surname)
                .ToListAsync();

            return result;
        }

        public async Task<UserDto> GetUser(int? id)
        {
            UserDto? user = _mapper.Map<UserDto>(await _context.Users.AsNoTracking().FirstOrDefaultAsync(i => i.UserId == id));
            if (user is null) throw new BadHttpRequestException("User not found");
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

        public async Task<bool> DeleteUser(int? userId)
        {
            User? userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (userToDelete is null) throw new BadHttpRequestException("User not found");
            return true;
        }

        public async Task<User> UpdateUser(User updateUserDto, int? userId)
        {
            User? currentUser = await _context.Users.SingleAsync(r => r.UserId == userId);

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
