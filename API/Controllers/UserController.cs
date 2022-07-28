using API.DTOs.Users;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class UserController : ApplicationController
    {
        [HttpGet("Infomations/{Id}")]
        [Authorize]
        public async Task<IActionResult> GetOne([FromRoute] Guid Id, [FromServices] UserService userService)
        {
            return Ok(await userService.GetOne(Id));
        }

        [HttpGet("{AccessToken}")]
        [Authorize]
        public async Task<IActionResult> GetInfo([FromRoute] string AccessToken, [FromServices] UserService userService)
        {
            return Ok(await userService.GetUserInfo(AccessToken));
        }

        [HttpPost("regist")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO request, [FromServices] UserService userService)
        {
            var user = await userService.CreateUser(request);
            return CreatedAtAction(nameof(GetOne), new { Id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAccountDTO request, [FromServices] UserService userService)
        {
            return Ok(await userService.ValidateUser(request));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] UserTokenDTO request, [FromServices] UserService userService)
        {
            return Ok(await userService.RefreshToken(request));
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO request, [FromServices] UserService userService)
        {
            await userService.UpdateUser(request);
            return NoContent();
        }
    }
}