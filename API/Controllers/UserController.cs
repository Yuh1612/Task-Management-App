using API.DTOs.Users;
using API.Exceptions;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class UserController : ApplicationController
    {
        public UserController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddUserRequest request,
            [FromServices] UserService userService)
        {
            try
            {
                var response = await userService.AddUserAsync(request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request,
            [FromServices] UserService userService)
        {
            try
            {
                var response = await userService.ValidateUser(request);
                return Ok(response);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidUserPasswordException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request,
            [FromServices] UserService userService)
        {
            try
            {
                var response = await userService.RefreshToken(request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request,
            [FromServices] UserService userService)
        {
            try
            {
                var response = await userService.UpdateUser(request, GetCurrentUserId());
                return Ok(response);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet("{Id}")]
        [Authorize]
        public async Task<IActionResult> GetOne([FromRoute] GetOneRequest request,
            [FromServices] UserService userService)
        {
            try
            {
                var response = await userService.GetOne(request);
                return Ok(response);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}