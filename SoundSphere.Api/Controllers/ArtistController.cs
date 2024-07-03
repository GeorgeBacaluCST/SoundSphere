using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService) => _artistService = artistService;

        [HttpGet] public IActionResult GetAll() => Ok(_artistService.GetAll());

        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_artistService.GetById(id));

        [HttpPost] public IActionResult Add(ArtistDto artistDto)
        {
            ArtistDto addedArtistDto = _artistService.Add(artistDto);
            return CreatedAtAction(nameof(GetById), new { addedArtistDto.Id }, addedArtistDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(ArtistDto artistDto, Guid id) => Ok(_artistService.UpdateById(artistDto, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_artistService.DeleteById(id));
    }
}