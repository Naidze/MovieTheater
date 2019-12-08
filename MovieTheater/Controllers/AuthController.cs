using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTheater.Models.ViewModels;
using MovieTheater.Services;
using System.Threading.Tasks;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken(LoginViewModel model)
        {
            string token = await _authService.RequestToken(model);

            if (token == null)
                return Unauthorized("Failed to get token for user: " + model.UserName);

            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        public IActionResult IsAuth()
        {
            return Ok(true);
        }
    }
}