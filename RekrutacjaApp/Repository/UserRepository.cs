
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class UserRepository :GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;      
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context,IDistributedCache cache, IMapper mapper) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public bool VerifyName(string firstName, string lastName)
        {
            bool pairExist = _context.Users.Any(u => u.Name == firstName && u.Surname == lastName);

            if (pairExist) return false;

            return true;
        }

        //public async Task<List<UserDto>> GetUsers(GetUsersQuery command)
        //{
        //    string key = JsonConvert.SerializeObject(command.queryParams);
        //    List<UserDto>? users = await _cache.GetRecordAsync<List<UserDto>>(key);
        //    if (users is null)
        //    {


        //        if (command.queryParams.CarLicense is not null)
        //        {
        //            query = query.Where(u => u.CarLicense == command.queryParams.CarLicense).AsQueryable();
        //        };

        //        if (command.queryParams.Gender is not null)
        //        {
        //            query = query.Where(u => u.Gender == command.queryParams.Gender).AsQueryable();
        //        };

        //        var totalCount = await query.CountAsync();
        //        users = await query
        //            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)

        //        await _cache.SetRecordAsync(key, users);
        //    }
        //    return users;
        //}

        public async Task<User> AddAttribute(CustomAttributeDto command,int userId)
        {
            User? foundedUser = await _context.Users
                .Include(a => a.CustomAttributes)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (foundedUser is null) throw new BadHttpRequestException("User not found");

            CustomAttribute newAttribute = new CustomAttribute()
            {
                UserId = foundedUser.UserId,
                Name = command.Name,
                Value = command.Value
            };
            await _cache.DeleteRecordAsync<User>($"User_{userId}");
            await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
            await _context.CustomAttributes.AddAsync(newAttribute);
            await _context.SaveChangesAsync();
            return foundedUser;
        }

        public async Task<bool> RemoveAttribute(RemoveAttributeCommand command)
        {
            var customAttribute = await _context.CustomAttributes.FirstOrDefaultAsync(i => i.CustomAttributeId == command.Id);
            if (customAttribute is null) throw new BadHttpRequestException("User not found");
            _context.CustomAttributes.Remove(customAttribute);
            await _context.SaveChangesAsync();
            await _cache.DeleteRecordAsync<User>($"User_{customAttribute.UserId}");
            await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
            return true;
        }

        //public async Task<UserDto> GetUser(GetUserQuery command)
        //{

        //    UserDto? user = await _cache.GetRecordAsync<UserDto>($"User_{command.userId}");
        //    if (user is null)
        //    {
        //        user = await _context.Users.Include(c => c.CustomAttributes)
        //            .AsNoTracking()
        //            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
        //            .FirstOrDefaultAsync(i => i.UserId == command.userId);
        //        await _cache.SetRecordAsync($"User_{command.userId}", user);
        //    }

        //    if (user is null) throw new BadHttpRequestException("User not found");
        //    return user;
        //}
        //public async Task<bool> CreateUser(CreateUserCommand command)
        //{
        //    User newUser = new User()
        //    {
        //        Name = command.user.Name,
        //        Surname = command.user.Surname,
        //        BirthDate = command.user.BirthDate,
        //        Gender = command.user.Gender,
        //        CarLicense = command.user.CarLicense
        //    };
        //    await _context.Users.AddAsync(newUser);
        //    await _context.SaveChangesAsync();

        //    await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
        //    return true;
        //}

        //public async Task<bool> DeleteUser(DeleteUserCommand command)
        //{
        //    User? userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.UserId == command.UserId);
        //    if (userToDelete is null) throw new BadHttpRequestException("User not found");
        //    _context.Remove(userToDelete);
        //    await _context.SaveChangesAsync();
        //    await _cache.DeleteRecordAsync<User>($"User_{userToDelete.UserId}");
        //    await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
        //    return true;
        //}

        //public async Task<bool> UpdateUser(UpdateUserCommand command)
        //{

        //    var currentUser = await _context.Users.FirstOrDefaultAsync(r => r.UserId == command.UserId);
        //    if (currentUser is null) throw new BadHttpRequestException("Bad");

        //    currentUser.Name = command.user.Name;
        //    currentUser.Surname = command.user.Surname;
        //    currentUser.BirthDate = command.user.BirthDate;
        //    currentUser.Gender = command.user.Gender;
        //    await _context.SaveChangesAsync();
        //    await _cache.DeleteRecordAsync<User>($"User_{command.UserId}");
        //    await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
        //    return true;
        //}

        public async Task<List<User>> GetUsersForRaport()
        {
            List<User> allUsersForRaport = await _context.Users.ToListAsync();
            return allUsersForRaport;
        }
    }
}
