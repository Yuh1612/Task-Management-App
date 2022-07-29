using API.DTOs.Tasks;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TaskController : ApplicationController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid id,
            [FromServices] TaskService taskService)
        {
            return Ok(await taskService.GetOne(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] CreateTaskDTO request,
            [FromServices] TaskService taskService)
        {
            var task = await taskService.AddTask(request);
            return CreatedAtAction(nameof(GetOne), new { Id = task.Id }, task);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateTask([FromBody] TaskDetailDTO request,
            [FromServices] TaskService taskService)
        {
            await taskService.UpdateTask(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id,
            [FromServices] TaskService taskService)
        {
            await taskService.DeleteTask(id);
            return NoContent();
        }

        [HttpPost("todos")]
        public async Task<IActionResult> AddTodo([FromBody] CreateTodoDTO request,
            [FromServices] TaskService taskService)
        {
            await taskService.AddTodo(request);
            return NoContent();
        }

        [HttpDelete("todos/{id}")]
        public async Task<IActionResult> RemoveTodo([FromRoute] Guid id,
            [FromServices] TaskService taskService)
        {
            await taskService.RemoveTodo(id);
            return NoContent();
        }

        [HttpPost("attachments")]
        public async Task<IActionResult> AddAttachment([FromForm] CreateAttachmentDTO request,
           [FromServices] TaskService taskService)
        {
            await taskService.AddAttachment(request);
            return NoContent();
        }

        [HttpDelete("attachments/{id}")]
        public async Task<IActionResult> RemoveAttachment([FromRoute] Guid Id,
            [FromServices] TaskService taskService)
        {
            await taskService.RemoveAttachment(Id);
            return NoContent();
        }

        [HttpPost("members")]
        public async Task<IActionResult> AddMember([FromBody] AssigmentDTO request,
           [FromServices] TaskService taskService)
        {
            await taskService.AddMember(request);
            return NoContent();
        }

        [HttpDelete("members")]
        public async Task<IActionResult> RemoveMember([FromBody] AssigmentDTO request,
            [FromServices] TaskService taskService)
        {
            await taskService.RemoveMember(request);
            return NoContent();
        }

        [HttpPost("labels")]
        public async Task<IActionResult> AddLabel([FromBody] TaskLabelDTO request,
           [FromServices] TaskService taskService)
        {
            await taskService.AddLabel(request);
            return NoContent();
        }

        [HttpDelete("labels")]
        public async Task<IActionResult> RemoveLabel([FromBody] TaskLabelDTO request,
            [FromServices] TaskService taskService)
        {
            await taskService.RemoveLabel(request);
            return NoContent();
        }
    }
}