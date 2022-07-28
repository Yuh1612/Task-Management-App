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
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid Id,
            [FromServices] ProjectService projectService)
        {
            return Ok(await projectService.GetOne(Id));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid Id,
            [FromServices] ProjectService projectService)
        {
            return Ok(await projectService.GetOne(Id));
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectDetailDTO request,
            [FromServices] ProjectService projectService)
        {
            await projectService.UpdateProject(request);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProject([FromRoute] Guid Id,
            [FromServices] ProjectService projectService)
        {
            await projectService.DeleteProject(Id);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDTO request,
            [FromServices] ProjectService projectService)
        {
            var project = await projectService.CreateProject(request);
            return CreatedAtAction(nameof(GetOne), new { Id = project.Id }, project);
        }

        [HttpPost("Members")]
        public async Task<IActionResult> AddMember([FromBody] ProjectMemberDTO request,
            [FromServices] ProjectService projectService)
        {
            await projectService.AddMember(request);
            return NoContent();
        }

        [HttpDelete("Members")]
        public async Task<IActionResult> RemoveMember([FromBody] ProjectMemberDTO request,
            [FromServices] ProjectService projectService)
        {
            await projectService.RemoveMember(request);
            return NoContent();
        }

        [HttpGet("ListTasks/{Id}")]
        public async Task<IActionResult> GetOneListTask([FromRoute] Guid Id,
            [FromServices] ProjectService projectService)
        {
            var response = await projectService.GetOneListTask(Id);
            return Ok(response);
        }

        [HttpPost("ListTasks")]
        public async Task<IActionResult> CreateListTask([FromBody] CreateListTaskDTO request,
            [FromServices] ProjectService projectService)
        {
            await projectService.CreateListTask(request);
            return NoContent();
        }

        [HttpDelete("ListTasks/{Id}")]
        public async Task<IActionResult> RemoveListTask([FromRoute] Guid Id,
            [FromServices] ProjectService projectService)
        {
            await projectService.RemoveListTask(Id);
            return NoContent();
        }

        [HttpPatch("ListTasks")]
        public async Task<IActionResult> UpdateListTask([FromBody] ListTaskDetailDTO request,
            [FromServices] ProjectService projectService)
        {
            await projectService.UpdateListTask(request);
            return NoContent();
        }
    }
}