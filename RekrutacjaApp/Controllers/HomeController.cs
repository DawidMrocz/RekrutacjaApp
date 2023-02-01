using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyWebApplication.Dtos;
using RekrutacjaApp.ActionFilters;
using RekrutacjaApp.Commands;
using RekrutacjaApp.Data;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Models;
using RekrutacjaApp.Queries;
using RekrutacjaApp.Repositories;
using System.Diagnostics;
using System.Net;

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

        public HomeController
            (
                ILogger<HomeController> logger,
                IMemoryCache memoryCache,
                IMapper mapper,
                IMediator mediatr,
                IUserRepository userRepository,
                ApplicationDbContext context
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediatr = mediatr ?? throw new ArgumentNullException(nameof(mediatr));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _context = context;
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
            List<UserDto>? users = _mapper.Map<List<UserDto>>(await _mediatr.Send(getUsersQuery));
            if (users is null) NotFound("No users");
            return View(users);
        }

        [HttpGet]
        [CustomFilterAttribute]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UserDto>> Details([FromRoute] int id)
        {
            GetUserQuery getUserQuery = new()
            {
                userId= id
            };
            UserDto user = _mapper.Map<UserDto>(await _mediatr.Send(getUserQuery));
            if (user is null) return NotFound("User not found!");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidationFilter]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateUser([FromForm][Bind(include:"Name,Surname,BirthDate,Gender,CarLicense")] User createUser)
        {
            CreateUserCommand createUserCommand = new CreateUserCommand()
            {
                user = createUser,
            };
            await _mediatr.Send(createUserCommand);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddAttribute([FromForm][Bind(include:"Name,Value")] CustomAttributeDto myattribute, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            AddAttributeCommand addAttributeCommand = new()
            {
                Id = id,
                attribute = myattribute
            };
            await _mediatr.Send(addAttributeCommand);
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RemoveAttribute([FromRoute] int id)
        {
            RemoveAttributeCommand removeAttributeCommand = new RemoveAttributeCommand()
            {
                Id = id
            };
            await _mediatr.Send(removeAttributeCommand);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            DeleteUserCommand deleteUserCommand = new DeleteUserCommand()
                {
                    UserId= id
                };
                await _mediatr.Send(deleteUserCommand);    
                return RedirectToAction(nameof(Index));    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidationFilter]
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
                return RedirectToAction("Details", new { id = id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}