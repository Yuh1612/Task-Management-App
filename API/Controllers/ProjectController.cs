using API.DTOs.Projects;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize]
    public class ProjectController : ApplicationController
    {
        public ProjectController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOne([FromRoute] GetOneProjectRequest request,
            [FromServices] ProjectService projectService)
        {
            var response = await projectService.GetOne(GetCurrentUserId(), request);
            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectRequest request,
            [FromServices] ProjectService projectService)
        {
            var response = await projectService.UpdateProject(GetCurrentUserId(), request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] AddProjectRequest request,
            [FromServices] ProjectService projectService)
        {
            var response = await projectService.AddProject(GetCurrentUserId(), request);
            return Ok(response);
        }

        [HttpPost("Members")]
        public async Task<IActionResult> AddMember([FromBody] AddMemberRequest request,
            [FromServices] ProjectService projectService)
        {
            var response = await projectService.AddMember(GetCurrentUserId(), request);
            return Ok(response);
        }

        [HttpDelete("Members")]
        public async Task<IActionResult> RemoveMember([FromBody] RemoveMemberRequest request,
            [FromServices] ProjectService projectService)
        {
            var response = await projectService.RemoveMember(GetCurrentUserId(), request);
            return Ok(response);
        }
    }
}