using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService) => _playlistService = playlistService;

        [HttpGet] public IActionResult GetAll() => Ok(_playlistService.GetAll());

        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_playlistService.GetById(id));

        [HttpPost] public IActionResult Add(PlaylistDto playlistDto)
        {
            PlaylistDto addedPlaylistDto = _playlistService.Add(playlistDto);
            return CreatedAtAction(nameof(GetById), new { addedPlaylistDto.Id }, addedPlaylistDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(PlaylistDto playlistDto, Guid id) => Ok(_playlistService.UpdateById(playlistDto, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_playlistService.DeleteById(id));
    }
}