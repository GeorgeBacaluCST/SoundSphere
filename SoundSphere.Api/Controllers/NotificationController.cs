using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService) => _notificationService = notificationService;

        [HttpGet] public IActionResult GetAll() => Ok(_notificationService.GetAll());

        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_notificationService.GetById(id));

        [HttpPost] public IActionResult Add(NotificationDto notificationDto)
        {
            NotificationDto addedNotificationDto = _notificationService.Add(notificationDto);
            return CreatedAtAction(nameof(GetById), new { addedNotificationDto.Id }, addedNotificationDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(NotificationDto notificationDto, Guid id) => Ok(_notificationService.UpdateById(notificationDto, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_notificationService.DeleteById(id));
    }
}