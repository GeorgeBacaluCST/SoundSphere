using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;

        [HttpGet] public IActionResult GetAll() => Ok(_userService.GetAll());

        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_userService.GetById(id));

        [HttpPost] public IActionResult Add(UserDto userDto)
        {
            UserDto addedUserDto = _userService.Add(userDto);
            return CreatedAtAction(nameof(GetById), new { addedUserDto.Id }, addedUserDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(UserDto userDto, Guid id) => Ok(_userService.UpdateById(userDto, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_userService.DeleteById(id));
    }
}