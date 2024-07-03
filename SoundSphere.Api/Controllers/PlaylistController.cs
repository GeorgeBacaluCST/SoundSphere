using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

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

        [HttpPost] public IActionResult Add(Playlist playlist)
        {
            Playlist addedPlaylist = _playlistService.Add(playlist);
            return CreatedAtAction(nameof(GetById), new { addedPlaylist.Id }, addedPlaylist);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Playlist playlist, Guid id) => Ok(_playlistService.UpdateById(playlist, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_playlistService.DeleteById(id));
    }
}