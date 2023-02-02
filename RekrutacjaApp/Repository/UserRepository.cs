
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

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool VerifyName(string firstName, string lastName)
        {
            bool pairExist = _context.Users.Any(u => u.Name == firstName && u.Surname == lastName);

            if (pairExist) return false;

            return true;
        }

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
            
            return true;
        }

        public async Task<List<User>> GetUsersForRaport()
        {
            List<User> allUsersForRaport = await _context.Users.ToListAsync();
            return allUsersForRaport;
        }
    }
}
