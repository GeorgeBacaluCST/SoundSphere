using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService) => _songService = songService;

        [HttpGet] public IActionResult GetAll() => Ok(_songService.GetAll());

        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_songService.GetById(id));

        [HttpPost] public IActionResult Add(SongDto songDto)
        {
            SongDto addedSongDto = _songService.Add(songDto);
            return CreatedAtAction(nameof(GetById), new { addedSongDto.Id }, addedSongDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(SongDto songDto, Guid id) => Ok(_songService.UpdateById(songDto, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_songService.DeleteById(id));
    }
}