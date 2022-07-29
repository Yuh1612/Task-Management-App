using API.DTOs.Projects;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class ProjectController : ApplicationController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] ProjectService projectService)
        {
            return Ok(await projectService.GetAll());
        }

        [HttpGet("project/{id}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid id,
            [FromServices] ProjectService projectService)
        {
            return Ok(await projectService.GetOne(id));
        }

        [HttpPatch("project")]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectDetailDTO request,
            [FromServices] ProjectService projectService)
        {
            await projectService.UpdateProject(request);
            return NoContent();
        }

        [HttpDelete("project/{id}")]
        public async Task<IActionResult> DeleteProject([FromRoute] Guid id,
            [FromServices] ProjectService projectService)
        {
            await projectService.DeleteProject(id);
            return NoContent();
        }

        [HttpPost("project")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDTO request,
            [FromServices] ProjectService projectService)
        {
            var project = await projectService.CreateProject(request);
            return CreatedAtAction(nameof(GetOne), new { Id = project.Id }, project);
        }

        [HttpPost("project/members")]
        public async Task<IActionResult> AddMember([FromBody] ProjectMemberDTO request,
            [FromServices] ProjectService projectService)
        {
            await projectService.AddMember(request);
            return NoContent();
        }

        [HttpDelete("project/members")]
        public async Task<IActionResult> RemoveMember([FromBody] ProjectMemberDTO request,
            [FromServices] ProjectService projectService)
        {
            await projectService.RemoveMember(request);
            return NoContent();
        }

        [HttpGet("project/listtasks/{id}")]
        public async Task<IActionResult> GetOneListTask([FromRoute] Guid id,
            [FromServices] ProjectService projectService)
        {
            var response = await projectService.GetOneListTask(id);
            return Ok(response);
        }

        [HttpPost("project/listtasks")]
        public async Task<IActionResult> CreateListTask([FromBody] CreateListTaskDTO request,
            [FromServices] ProjectService projectService)
        {
            await projectService.CreateListTask(request);
            return NoContent();
        }

        [HttpDelete("project/listtasks/{id}")]
        public async Task<IActionResult> RemoveListTask([FromRoute] Guid id,
            [FromServices] ProjectService projectService)
        {
            await projectService.RemoveListTask(id);
            return NoContent();
        }

        [HttpPatch("project/listtasks")]
        public async Task<IActionResult> UpdateListTask([FromBody] ListTaskDetailDTO request,
            [FromServices] ProjectService projectService)
        {
            await projectService.UpdateListTask(request);
            return NoContent();
        }
    }
}