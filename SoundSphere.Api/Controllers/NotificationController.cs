using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

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

        [HttpPost] public IActionResult Add(Notification notification)
        {
            Notification addedNotification = _notificationService.Add(notification);
            return CreatedAtAction(nameof(GetById), new { addedNotification.Id }, addedNotification);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Notification notification, Guid id) => Ok(_notificationService.UpdateById(notification, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_notificationService.DeleteById(id));
    }
}