using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using MyWebApplication.Dtos;
using Newtonsoft.Json;
using RekrutacjaApp.ActionFilters;
using RekrutacjaApp.Commands;
using RekrutacjaApp.Data;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Helpers;
using RekrutacjaApp.Models;
using RekrutacjaApp.Queries;
using RekrutacjaApp.Repositories;
using System.Data.Common;
using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RekrutacjaApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediatr;
        private readonly IMemoryCache _memoryCache;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        public HomeController
            (
                ILogger<HomeController> logger,
                IMemoryCache memoryCache,
                IMapper mapper,
                IMediator mediatr,
                IUserRepository userRepository,
                ApplicationDbContext context,
                IDistributedCache cache
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediatr = mediatr ?? throw new ArgumentNullException(nameof(mediatr));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _context = context;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        [HttpPost]
        public async Task<IActionResult> GenerateRaport()
        {
            List<UserDto>? users = _mapper.Map<List<UserDto>>(await _context.Users.AsNoTracking().ToListAsync());
            var reportDate = DateTime.Now;
            string desktopPath = Environment.GetFolderPath(
                         System.Environment.SpecialFolder.DesktopDirectory);

            string name = $"{reportDate.ToString("yyyy-MM-dd_HH-mm-ss")}.txt";

            using (var writer = new StreamWriter(Path.Combine(desktopPath, name)))
            {
                writer.WriteLine("Imię,Nazwisko,Data urodzenia,Płeć,Tytuł,Wiek");
                foreach (var user in users)
                {
                    writer.WriteLine("{0},{1},{2},{3},{4}", user.Title, user.DisplayName, user.BirthDate.ToString("yyyy-MM-dd"), user.Age, user.Gender);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Index([FromQuery] QueryParams? stringParameters)
        {
            GetUsersQuery getUsersQuery = new()
            {
                queryParams = stringParameters
            };

            string key = JsonConvert.SerializeObject(stringParameters);
            List<UserDto>? users = await _cache.GetRecordAsync<List<UserDto>>(key);

                if (users is null)
                {
          
                        users = _mapper.Map<List<UserDto>>(await _mediatr.Send(getUsersQuery));

                        await _cache.SetRecordAsync(key, users);
                    

                }         
           
            
            return View(users);
        }

        [HttpGet]
        [IdProvidedValidation]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UserDto>> Details([FromRoute] int id)
        {
            GetUserQuery getUserQuery = new()
            {
                userId = id
            };
            UserDto? user = await _cache.GetRecordAsync<UserDto>($"User_{id}");
            if (user is null)
            {
                user = _mapper.Map<UserDto>(await _mediatr.Send(getUserQuery));
                await _cache.SetRecordAsync($"User_{id}", user);
            }
            if (user is null) return NotFound("User not found!");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidationFilter(DTOName = "createUser")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateUser([FromForm][Bind(include:"Name,Surname,BirthDate,Gender,CarLicense")] User createUser)
        {

            CreateUserCommand createUserCommand = new CreateUserCommand()
            {
                user = createUser,
            };
            await _mediatr.Send(createUserCommand);
            await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdProvidedValidation]
        [ValidationFilter(DTOName = "myattribute")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddAttribute([FromForm][Bind(include:"Name,Value")] CustomAttributeDto myattribute, [FromRoute] int id)
        {
            AddAttributeCommand addAttributeCommand = new()
            {
                Id = id,
                attribute = myattribute
            };
            await _mediatr.Send(addAttributeCommand);
            await _cache.DeleteRecordAsync<User>($"User_{id}");
            await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdProvidedValidation]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RemoveAttribute([FromRoute] int id, [FromQuery] int userId)
        {
            RemoveAttributeCommand removeAttributeCommand = new RemoveAttributeCommand()
            {
                Id = id
            };
            await _mediatr.Send(removeAttributeCommand);
            await _cache.DeleteRecordAsync<User>($"User_{userId}");
            await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdProvidedValidation]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            DeleteUserCommand deleteUserCommand = new DeleteUserCommand()
                {
                    UserId= id
                };
                await _mediatr.Send(deleteUserCommand);
            await _cache.DeleteRecordAsync<User>($"User_{id}");
            await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
            return RedirectToAction(nameof(Index));    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidationFilter(DTOName = "updateUser")]
        [IdProvidedValidation]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateUser([FromForm][Bind(include:"Name,Surname,BirthDate,Gender,CarLicense")] User updateUser, [FromRoute] int id)
        {
                UpdateUserCommand updateUserCommand = new UpdateUserCommand()
                {
                    UserId =id,
                    user= updateUser,
                };
                await _mediatr.Send(updateUserCommand);
                await _cache.DeleteRecordAsync<User>($"User_{id}");
                await _cache.DeleteRecordAsync<List<User>>(JsonConvert.SerializeObject(new QueryParams()));
            return RedirectToAction("Details", new { id = id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}