using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService) => _albumService = albumService;

        [HttpGet] public IActionResult GetAll() => Ok(_albumService.GetAll());

        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_albumService.GetById(id));

        [HttpPost] public IActionResult Add(Album album)
        {
            Album addedAlbum = _albumService.Add(album);
            return CreatedAtAction(nameof(GetById), new { addedAlbum.Id }, addedAlbum);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Album album, Guid id) => Ok(_albumService.UpdateById(album, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_albumService.DeleteById(id));
    }
}