using FintechApp.dtos;
using FintechApp.Extensions;
using FintechApp.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace FintechApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository<User> _userRepo;
        private readonly TokenService _tokenservice;

        public UserController(IConfiguration config, IUserRepository<User> userRepo, TokenService tokenService )
        {

            _config = config;
            _userRepo = userRepo;
            _tokenservice = tokenService;
        }
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateUser([FromBody] NewUser user, IValidator<NewUser> validator)
        {
            var validationResults = await validator.ValidateAsync(user);

            if (!validationResults.IsValid)
            {

                return ValidationProblem(validationResults.ToModelStateDictionary());

            }
            var salt = _config.GetValue<string>("Salt");

            var userExist = await _userRepo.UserExist(user.Email);

            if (userExist != null)
            {



                return Problem(
                        type: "/docs/errors/forbidden",
                        title: "Registeration Error",
                        detail: $"Email Already taken",
                        statusCode: StatusCodes.Status403Forbidden,
                        instance: HttpContext.Request.Path
                );
            }

            var hashedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: Convert.FromBase64String(salt!),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            var newUser = new User
            {
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Password = hashedPwd
            };

            await _userRepo.Create(newUser);

            return Created();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UserLogin([FromBody] UserLogin user, IValidator<UserLogin> validator)
        {
            var validationResults = await validator.ValidateAsync(user);

            if (!validationResults.IsValid)
            {

                return ValidationProblem(validationResults.ToModelStateDictionary());

            }
            var salt = _config.GetValue<string>("Salt");

            var userExist = await _userRepo.UserExist(user.Email);

            if (userExist == null)
            {

                return Forbid("Wrong Email/Password Combination");
            }

            var hashedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: Convert.FromBase64String(salt!),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));


            if (userExist.Password != hashedPwd)
            {
                

                return Problem(
                        type: "/docs/errors/forbidden",
                        title: "Authentication Error",
                        detail: $"Wrong Email/Password Combination",
                        statusCode: StatusCodes.Status403Forbidden,
                        instance: HttpContext.Request.Path
                );
            }



            return Ok(_tokenservice.CreateToken(userExist));
        }
    }
}
