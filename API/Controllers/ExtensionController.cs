using API.DTOs.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public ExtensionController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("Histories")]
        public async Task<IActionResult> GetAllHistory([FromServices] IUnitOfWork unitOfWork)
        {
            return Ok(await unitOfWork.historyRepository.GetAllAsync());
        }

        [HttpGet("Labels")]
        public async Task<IActionResult> GetAllLabel([FromServices] IUnitOfWork unitOfWork)
        {
            return Ok(_mapper.Map<List<LabelDTO>>(await unitOfWork.labelRepository.GetAllAsync()));
        }
    }
}