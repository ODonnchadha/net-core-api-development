using AutoMapper;
using HotelListing.DTOs.Users;
using HotelListing.Interfaces.Managers;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [ApiController(), Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthManager authManager;
        private readonly ILogger<AccountsController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public AccountsController(
            IAuthManager authManager,
            ILogger<AccountsController> logger,
            IMapper mapper,
            UserManager<User> userManager)
        {
            this.authManager = authManager;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpPost(), Route("Login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            logger.LogInformation($"Login attempt for {userLogin.Email}.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (await authManager.ValidateUserAsync(userLogin) == false)
                {
                    logger.LogInformation($"User login attempt failed for {userLogin.Email}.");
                    return Unauthorized(userLogin);
                }
            }
            catch (Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }

            return Accepted(new { Token = authManager.CreateToken() });
        }

        [HttpPost(), Route("Register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
        {
            logger.LogInformation($"Registration attempt for {userRegister.Email}.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = mapper.Map<User>(userRegister);
                user.UserName = userRegister.Email;
                var result = await userManager.CreateAsync(user, userRegister.Password);

                if (!result.Succeeded)
                {
                    logger.LogInformation($"User registration attempt failed for {userRegister.Email}.");

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                await userManager.AddToRolesAsync(user, userRegister.Roles);
            }
            catch (Exception exception)
            {
                logger.LogError(this.GetType().FullName, exception);
                return StatusCode(500, "Internal server error. Please try again later.");
            }

            return Accepted();
        }
    }
}
