using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

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

        [HttpPost] public IActionResult Add(Artist artist)
        {
            Artist addedArtist = _artistService.Add(artist);
            return CreatedAtAction(nameof(GetById), new { addedArtist.Id }, addedArtist);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Artist artist, Guid id) => Ok(_artistService.UpdateById(artist, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_artistService.DeleteById(id));
    }
}