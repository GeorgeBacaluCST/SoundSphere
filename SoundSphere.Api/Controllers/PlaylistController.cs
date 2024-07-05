using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using System.Net.Mime;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService) => _playlistService = playlistService;

        /// <summary>Get all playlists</summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult GetAll() => Ok(_playlistService.GetAll());

        /// <summary>Get playlist by ID</summary>
        /// <param name="id">Playlist fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_playlistService.GetById(id));

        /// <summary>Add playlist</summary>
        /// <param name="playlistDto">PlaylistDTO to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(PlaylistDto playlistDto)
        {
            PlaylistDto addedPlaylistDto = _playlistService.Add(playlistDto);
            return CreatedAtAction(nameof(GetById), new { addedPlaylistDto.Id }, addedPlaylistDto);
        }

        /// <summary>Update playlist by ID</summary>
        /// <param name="playlistDto">PlaylistDTO to update</param>
        /// <param name="id">Playlist updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(PlaylistDto playlistDto, Guid id) => Ok(_playlistService.UpdateById(playlistDto, id));

        /// <summary>Soft delete playlist by ID</summary>
        /// <param name="id">Playlist deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_playlistService.DeleteById(id));
    }
}