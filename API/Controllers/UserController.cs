using API.DTOs.Users;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ApplicationController
    {
        [HttpGet("user/infomations/{id}")]
        [Authorize]
        public async Task<IActionResult> GetOne([FromRoute] Guid id, [FromServices] UserService userService)
        {
            return Ok(await userService.GetOne(id));
        }

        [HttpGet("user/{accesstoken}")]
        [Authorize]
        public async Task<IActionResult> GetInfo([FromRoute] string accesstoken, [FromServices] UserService userService)
        {
            return Ok(await userService.GetUserInfo(accesstoken));
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO request, [FromServices] UserService userService)
        {
            var user = await userService.CreateUser(request);
            return CreatedAtAction(nameof(GetOne), new { Id = user.Id }, user);
        }

        [HttpPost("user/login")]
        public async Task<IActionResult> Login([FromBody] UserAccountDTO request, [FromServices] UserService userService)
        {
            return Ok(await userService.ValidateUser(request));
        }

        [HttpPost("user/refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] UserTokenDTO request, [FromServices] UserService userService)
        {
            return Ok(await userService.RefreshToken(request));
        }

        [HttpPatch("user")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO request, [FromServices] UserService userService)
        {
            await userService.UpdateUser(request);
            return NoContent();
        }
    }
}