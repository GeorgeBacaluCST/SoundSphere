using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;

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

        [HttpPost] public IActionResult Add(AlbumDto albumDto)
        {
            AlbumDto addedAlbumDto = _albumService.Add(albumDto);
            return CreatedAtAction(nameof(GetById), new { addedAlbumDto.Id }, addedAlbumDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(AlbumDto albumDto, Guid id) => Ok(_albumService.UpdateById(albumDto, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_albumService.DeleteById(id));
    }
}