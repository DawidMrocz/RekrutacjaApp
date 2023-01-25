﻿using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Models;
using RekrutacjaApp.Repositories;
using System.Diagnostics;
using System.Net;

namespace RekrutacjaApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        //ZMIENIC !
        [HttpPost]
        public IActionResult AddToDictionary(List<KeyValuePair<string, string>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                //_myDictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
            return RedirectToAction("Index");
        }



        [HttpGet]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GenerateRaport()
        {
            List<User>? users = await _userRepository.GetUsersForRaport();

            var reportDate = DateTime.Now;
            var reportName = $"report_{reportDate.ToString("yyyy-MM-dd_HH-mm-ss")}.csv";
            using (var writer = new StreamWriter(reportName))
            {
                writer.WriteLine("Imię,Nazwisko,Data urodzenia,Płeć,Tytuł,Wiek");
                foreach (var user in users)
                {
                    writer.WriteLine("{0},{1},{2},{3},{4},{5}", user.Name, user.Surname, user.BirthDate.ToString("yyyy-MM-dd"), user.Gender, user.Title, user.Age);
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

        [HttpGet]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<List<User>> SearchUsers([FromQuery] SearchQuery searchQuery)
        {
            List<User>? users = await _userRepository.SearchUsers(searchQuery);
            return users;
        }

        [HttpGet]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PagedResult<List<User>>>> GetUsers([FromQuery] QueryParams queryParams)
        {
            PagedResult<List<User>>? users = await _userRepository.GetUsers(queryParams);
            return View(users);
        }

        [HttpGet]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<User>> GetUser([FromRoute] Guid? id)
        {

            if (id is null) return NotFound();

            User? user = await _userRepository.GetUser(id);

            if (user is null) return NotFound();

            return user;
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateUser([FromForm][Bind("Name,Surname,BirthDate,Gender")] User createUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            User? newUser = await _userRepository.CreateUser(createUserDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<bool>> DeleteUser([FromRoute] Guid? userId)
        {
            if (userId is null) return NotFound();
            await _userRepository.DeleteUser(userId);
            return true;
        }

        [HttpPut]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<User>> UpdateUser([FromBody] User updateUserDto, [FromRoute] Guid? userId)
        {
            User? currentUser = await _userRepository.UpdateUser(updateUserDto, userId);

            if (currentUser is null) return NotFound("Not found");

            currentUser.Name = updateUserDto.Name;
            currentUser.Surname = updateUserDto.Surname;
            currentUser.BirthDate = updateUserDto.BirthDate;
            currentUser.Gender = updateUserDto.Gender;

            return Ok(currentUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}