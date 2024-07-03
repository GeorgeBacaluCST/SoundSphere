using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorityController : ControllerBase
    {
        private readonly IAuthorityService _authorityService;

        public AuthorityController(IAuthorityService authorityService) => _authorityService = authorityService;

        [HttpGet] public IActionResult GetAll() => Ok(_authorityService.GetAll());

        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_authorityService.GetById(id));

        [HttpPost] public IActionResult Add(AuthorityDto authorityDto)
        {
            AuthorityDto addedAuthorityDto = _authorityService.Add(authorityDto);
            return CreatedAtAction(nameof(GetById), new { addedAuthorityDto.Id }, addedAuthorityDto);
        }
    }
}