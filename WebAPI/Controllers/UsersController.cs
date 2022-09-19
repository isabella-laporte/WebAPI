using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.AuthorizationAndAuthentication;
using WebAPI.Interfaces;


namespace WebAPI.Controllers
{
    [Route("login")]
    [ApiController]
    [AllowAnonymous]

    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        private readonly GenerateToken _generateToken;
        private readonly IConfiguration _configuration;

        public UsersController(IUsersRepository repository, GenerateToken generateToken, IConfiguration configuration)
        {
            _repository = repository;
            _generateToken = generateToken;
            _configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] Authenticate request)
        {
            var username = _configuration["UserAuthentication:login"];
            var password = _configuration["UserAuthentication:password"];
            var user = await _repository.Get(request.Username, request.Password);
            if (username != request.Username && password != request.Password)
                return Unauthorized(new { message = "Username or password is invalid" });

            var token = _generateToken.GenerateJwt(user);
            user.Password = "";
            return Ok(new { user = user, token = token });
        }
    }
}

