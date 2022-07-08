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
            try
            {
                var response = await projectService.GetOne(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectRequest request,
            [FromServices] ProjectService projectService)
        {
            try
            {
                var response = await projectService.UpdateProject(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}