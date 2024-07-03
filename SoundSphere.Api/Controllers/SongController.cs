using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

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

        [HttpPost] public IActionResult Add(Song song)
        {
            Song addedSong = _songService.Add(song);
            return CreatedAtAction(nameof(GetById), new { addedSong.Id }, addedSong);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Song song, Guid id) => Ok(_songService.UpdateById(song, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_songService.DeleteById(id));
    }
}