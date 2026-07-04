using Microsoft.AspNetCore.Mvc;

using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using School_Project_API.Services.Interfaces;
using School_Project_API.DTO;
using Microsoft.AspNetCore.Authorization;


namespace StudentApi.Controllers
{
    // This controller is responsible for authentication-related actions,
    // such as logging in and issuing JWT tokens.
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {


        private readonly IAuthService _authService;


        public AuthController(IAuthService authService)
        {

            _authService = authService;
        }

        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {

            var success = await _authService.RegisterAsync(registerDTO);

            if (!success)
                return BadRequest("Email already in user");


            return Ok("User registered successfully");

        }


        // This endpoint handles user login.
        // It verifies credentials and returns a JWT token if login succeeds.
        [HttpPost("login")]
        public async Task  <IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var token = await _authService.LoginAsync(loginDTO);

            if(token == null)
                return Unauthorized("Invalid email or password");

            return Ok(new { token });

        }
    }
}
