using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

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

        [HttpPost] public IActionResult Add(User user)
        {
            User addedUser = _userService.Add(user);
            return CreatedAtAction(nameof(GetById), new { addedUser.Id }, addedUser);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(User user, Guid id) => Ok(_userService.UpdateById(user, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_userService.DeleteById(id));
    }
}