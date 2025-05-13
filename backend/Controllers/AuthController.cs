using Microsoft.AspNetCore.Mvc;

using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public IActionResult Signup(UserDTO userDto)
        {
            var result = _authService.RegisterUser(userDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login(UserDTO userDto)
        {
            var result = _authService.AuthenticateUser(userDto);
            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }
            return Ok(result);
        }
    }
}
