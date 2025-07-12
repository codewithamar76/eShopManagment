using AuthService.Application.DTOs;
using AuthService.Application.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAppService _appService;
        public AuthController(IUserAppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignUpDTO signUp, string role)
        {
            bool res = await _appService.SignupUserAsync(signUp, role);
            if (res)
            {
                return CreatedAtAction("Signup","User registered successfully....");
            }
            return BadRequest("User already exists or registration failed.");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            UserDTO user = await _appService.LoginUserAsync(login);
            if (user != null)
            {
                return Ok(user);
            }

            return BadRequest();
        }
    }
}
