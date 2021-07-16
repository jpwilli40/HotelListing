using AutoMapper;
using HotelListing.Controllers.Data;
using HotelListing.DTOModels;
using HotelListing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<APIUser> _userManager;  //built in library from IdentityCore, uses whatever class (object of IdentityUser which is default) you set in configuration.  In our case in our ServiceExtensions clas for AddIdentiyCore, RoleManager is also another option
        //private readonly SignInManager<APIUser> _signInManager; //built in library from IdentityCore
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        public AccountController(UserManager<APIUser> userManager,
            ILogger<AccountController> logger,
            IMapper mapper,
            IAuthManager authManager)
        {
            _userManager = userManager;
           // _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)  
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               var user = _mapper.Map<APIUser>(userDTO);
               user.UserName = userDTO.Email;
               var result = await _userManager.CreateAsync(user, userDTO.Password);

               if(!result.Succeeded)
               {
                  foreach (var error in result.Errors)
                  {
                        ModelState.AddModelError(error.Code, error.Description);
                  }

                  return BadRequest(ModelState);
               }

                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted();
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, $"Something Went Wrong IN the {nameof(Register)}");
               return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)  //ensures that we are only looking for fields included in LoginUserDTO
        {
            _logger.LogInformation($"Login Attempt for {loginUserDTO.Email} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await _authManager.ValidateUser(loginUserDTO))
                {
                    return Unauthorized();
                }

                return Accepted(new { Token = await _authManager.CreateToken() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong IN the {nameof(Login)}");
                return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);
            }
        }
    }
}
