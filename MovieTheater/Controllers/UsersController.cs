using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Models;
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
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;

        public UsersController(IUserRepository userRepository, IUserService userService, IAuthService authService, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userService = userService;
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            if (!await _userRepository.UserExists(model.UserName))
                return StatusCode(401, "You have entered an invalid username or password!");

            if (await _userService.Login(model))
                return Ok(_authService.RequestToken(model));
            return StatusCode(401, "You have entered an invalid username or password!");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (model.UserName == null || model.Password == null || model.Email == null)
                return UnprocessableEntity("UserName, Password and/or Email is missing.");

            // Checking for duplicates usernames
            if (await _userRepository.UserExists(model.UserName))
                return StatusCode(422, "Username is already taken!");

            // Check for username length
            if (!Regex.IsMatch(model.UserName, @"^.{4,11}$"))
                return StatusCode(422, "Username must be between 4 and 11 symbols long!");

            // Check if email is valid
            if (!Regex.IsMatch(model.Email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                return StatusCode(422, "Entered email is invalid!");

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
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            List<object> usrs = new List<object>();
            var users = _userService.GetAllUsers();
            await _userManager.Users.ForEachAsync(async user =>
            {
                var roles = await _userManager.GetRolesAsync(user);
                usrs.Add(new
                {
                    user.Id,
                    user.UserName,
                    roles
                });
            });
            return Ok(usrs);
        }
    }
}