using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyWebApplication.Dtos;
using RekrutacjaApp.Commands;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Models;
using RekrutacjaApp.Queries;
using RekrutacjaApp.Repositories;
using RekrutacjaApp.Repository;
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
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController
            (
                ILogger<HomeController> logger,
                IMemoryCache memoryCache, 
                IUnitOfWork unitOfWork, 
                IMapper mapper, 
                IMediator mediatr, 
                IUserRepository userRepository
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediatr = mediatr ?? throw new ArgumentNullException(nameof(mediatr));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRaport()
        {
            List<UserDto>? users = await _mediatr.Send(new GetUsersQuery());
            var reportDate = DateTime.Now;
            var reportName = $"report_{reportDate.ToString("yyyy-MM-dd_HH-mm-ss")}.csv";
            using (var writer = new StreamWriter(reportName))
            {
                writer.WriteLine("Imię,Nazwisko,Data urodzenia,Płeć,Tytuł,Wiek");
                foreach (var user in users)
                {
                    writer.WriteLine("{0},{1},{2},{3},{4}", user.Title,user.DisplayName, user.BirthDate.ToString("yyyy-MM-dd"), user.Age, user.Gender);
                }
            }
            return Ok();
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<ActionResult> VerifyName(string firstName, string lastName)
        {
            if (await _userRepository.VerifyName(firstName, lastName))
            {
                return Json($"A user named {firstName} {lastName} already exists.");
            }
            return Json(true);
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //public async Task<ActionResult<List<UserDto>>> Index([FromQuery] QueryParams? stringParameters)
        //{
        //    GetUsersQuery getUsersQuery = new()
        //    {
        //        queryParams = stringParameters
        //    };
        //    List<UserDto>? users = await _mediatr.Send(getUsersQuery);
        //    if(users is null) NotFound("No users");        
        //    return View(users);
        //}

        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<UserDto>>> Index([FromQuery] QueryParams? stringParameters)
        {
            GetUsersQuery getUsersQuery = new()
            {
                queryParams = stringParameters
            };
            List<UserDto>? users = await _mediatr.Send(getUsersQuery);
            if (users is null) NotFound("No users");
            return View(users);
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UserDto>> Details([FromRoute] int id)
        {
            GetUserQuery getUserQuery = new()
            {
                userId= id
            };
            UserDto user = await _mediatr.Send(getUserQuery);
            if (user is null) return NotFound("User not found!");
            return View(user);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateUser([FromForm][Bind(include:"Name,Surname,BirthDate,Gender")] User createUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            CreateUserCommand createUserCommand = new()
            {
                user = createUser,
            };
            await _mediatr.Send(createUserCommand);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<bool>> Delete([FromRoute] int userId)
        {
            try
            {
                DeleteUserCommand deleteUserCommand = new()
                {
                    UserId= userId
                };
                await _mediatr.Send(deleteUserCommand);    
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Index));
            }         
        }

        [HttpPut]
        [AutoValidateAntiforgeryToken]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<User>> UpdateUser([FromForm][Bind(include: "Name,Surname,BirthDate,Gender")] User updateUser, [FromRoute] int userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                UpdateUserCommand updateUserCommand = new()
                {
                    UserId = userId,
                    user= updateUser,
                };
                await _mediatr.Send(updateUserCommand);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}