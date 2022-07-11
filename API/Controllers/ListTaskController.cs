using API.DTOs.ListTasks;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize]
    public class ListTaskController : ApplicationController
    {
        public ListTaskController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOne([FromRoute] GetOneListTaskRequest request,
            [FromServices] ListTaskService listTaskService)
        {
            try
            {
                var response = await listTaskService.GetOne(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddListTask([FromBody] AddListTaskRequest request,
            [FromServices] ListTaskService listTaskService)
        {
            try
            {
                var response = await listTaskService.AddListTask(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateListTask([FromBody] UpdateListTaskRequest request,
            [FromServices] ListTaskService listTaskService)
        {
            try
            {
                var response = await listTaskService.UpdateListTask(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}