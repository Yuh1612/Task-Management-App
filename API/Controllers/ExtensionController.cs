using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize]
    public class ExtensionController : ApplicationController
    {
        [HttpGet("Histories")]
        public async Task<IActionResult> GetAllHistory([FromServices] IUnitOfWork unitOfWork)
        {
            return Ok(await unitOfWork.historyRepository.GetAllAsync());
        }

        [HttpGet("Labels")]
        public async Task<IActionResult> GetAllLabel([FromServices] IUnitOfWork unitOfWork)
        {
            return Ok(await unitOfWork.labelRepository.GetAllAsync());
        }
    }
}