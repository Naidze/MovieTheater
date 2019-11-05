using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Models.ViewModels;
using MovieTheater.Repositories;
using MovieTheater.Services;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UsersController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            if (!await _userRepository.UserExists(model.UserName))
                return StatusCode(401, "You have entered an invalid username or password!");

            //if (await _userService.Login(model))
            //    return Ok(_userService.RequestToken(model.UserName));
            return StatusCode(401, "You have entered an invalid username or password!");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            // Checking for duplicates usernames
            if (await _userRepository.UserExists(model.UserName))
                return StatusCode(422, "Username is already taken!");

            // Check for username length
            if (!Regex.IsMatch(model.UserName, @"^.{4,11}$"))
                return StatusCode(422, "Username must be between 4 and 11 symbols long!");

            IdentityResult result = await _userService.Register(model);
            if (!result.Succeeded)
            {
                IEnumerable<IdentityError> errors = result.Errors;
                return StatusCode(422, errors.Select(err => err.Description));
            }

            return Ok("You have registered successfully!");
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<object>> GetUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
    }
}