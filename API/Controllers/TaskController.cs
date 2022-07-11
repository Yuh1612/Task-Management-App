using API.DTOs.Tasks;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize]
    public class TaskController : ApplicationController
    {
        public TaskController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> AddTask([FromRoute] GetOneTaskRequest request,
            [FromServices] TaskService taskService)
        {
            try
            {
                var response = await taskService.GetOne(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] AddTaskRequest request,
            [FromServices] TaskService taskService)
        {
            try
            {
                var response = await taskService.AddTask(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequest request,
            [FromServices] TaskService taskService)
        {
            try
            {
                var response = await taskService.UpdateTask(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost("Todos")]
        public async Task<IActionResult> AddTodo([FromBody] AddTodoRequest request,
            [FromServices] TaskService taskService)
        {
            try
            {
                var response = await taskService.AddTodo(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpDelete("Todos/{Id}")]
        public async Task<IActionResult> RemoveTodo([FromRoute] RemoveTodoRequest request,
            [FromServices] TaskService taskService)
        {
            try
            {
                var response = await taskService.RemoveTodo(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost("Attachments")]
        public async Task<IActionResult> AddTodo([FromForm] AddAttachmentRequest request,
           [FromServices] TaskService taskService)
        {
            try
            {
                var response = await taskService.AddAttachment(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpDelete("Attachments/{Id}")]
        public async Task<IActionResult> RemoveAttachment([FromRoute] RemoveAttachmentRequest request,
            [FromServices] TaskService taskService)
        {
            try
            {
                var response = await taskService.RemoveAttachment(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost("Members")]
        public async Task<IActionResult> AddMember([FromBody] AddAssigneeRequest request,
           [FromServices] TaskService taskService)
        {
            try
            {
                var response = await taskService.AddMember(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpDelete("Members")]
        public async Task<IActionResult> RemoveMembert([FromBody] RemoveAssgineeRequest request,
            [FromServices] TaskService taskService)
        {
            try
            {
                var response = await taskService.RemoveMember(GetCurrentUserId(), request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}