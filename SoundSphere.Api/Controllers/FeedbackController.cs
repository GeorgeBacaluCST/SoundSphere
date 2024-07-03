using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;

namespace SoundSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService) => _feedbackService = feedbackService;

        [HttpGet] public IActionResult GetAll() => Ok(_feedbackService.GetAll());

        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_feedbackService.GetById(id));

        [HttpPost] public IActionResult Add(Feedback feedback)
        {
            Feedback addedFeedback = _feedbackService.Add(feedback);
            return CreatedAtAction(nameof(GetById), new { addedFeedback.Id }, addedFeedback);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(Feedback feedback, Guid id) => Ok(_feedbackService.UpdateById(feedback, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_feedbackService.DeleteById(id));
    }
}