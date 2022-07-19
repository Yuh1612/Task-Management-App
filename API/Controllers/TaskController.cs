﻿using API.DTOs.Tasks;
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
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid Id,
            [FromServices] TaskService taskService)
        {
            return Ok(await taskService.GetOne(Id));
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

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid Id,
            [FromServices] TaskService taskService)
        {
            await taskService.DeleteTask(Id);
            return NoContent();
        }

        [HttpPost("Todos")]
        public async Task<IActionResult> AddTodo([FromBody] CreateTodoDTO request,
            [FromServices] TaskService taskService)
        {
            await taskService.AddTodo(request);
            return NoContent();
        }

        [HttpDelete("Todos/{Id}")]
        public async Task<IActionResult> RemoveTodo([FromRoute] Guid Id,
            [FromServices] TaskService taskService)
        {
            await taskService.RemoveTodo(Id);
            return NoContent();
        }

        [HttpPost("Attachments")]
        public async Task<IActionResult> AddAttachment([FromForm] CreateAttachmentDTO request,
           [FromServices] TaskService taskService)
        {
            await taskService.AddAttachment(request);
            return NoContent();
        }

        [HttpDelete("Attachments/{Id}")]
        public async Task<IActionResult> RemoveAttachment([FromRoute] Guid Id,
            [FromServices] TaskService taskService)
        {
            await taskService.RemoveAttachment(Id);
            return NoContent();
        }

        [HttpPost("Members")]
        public async Task<IActionResult> AddMember([FromBody] AssigmentDTO request,
           [FromServices] TaskService taskService)
        {
            await taskService.AddMember(request);
            return NoContent();
        }

        [HttpDelete("Members")]
        public async Task<IActionResult> RemoveMember([FromBody] AssigmentDTO request,
            [FromServices] TaskService taskService)
        {
            await taskService.RemoveMember(request);
            return NoContent();
        }

        [HttpPost("Labels")]
        public async Task<IActionResult> AddLabel([FromBody] TaskLabelDTO request,
           [FromServices] TaskService taskService)
        {
            await taskService.AddLabel(request);
            return NoContent();
        }

        [HttpDelete("Labels")]
        public async Task<IActionResult> RemoveLabel([FromBody] TaskLabelDTO request,
            [FromServices] TaskService taskService)
        {
            await taskService.RemoveLabel(request);
            return NoContent();
        }
    }
}