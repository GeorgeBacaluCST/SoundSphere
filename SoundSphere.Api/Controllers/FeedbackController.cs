using Microsoft.AspNetCore.Mvc;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;

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

        [HttpPost] public IActionResult Add(FeedbackDto feedbackDto)
        {
            FeedbackDto addedFeedbackDto = _feedbackService.Add(feedbackDto);
            return CreatedAtAction(nameof(GetById), new { addedFeedbackDto.Id }, addedFeedbackDto);
        }

        [HttpPut("{id}")] public IActionResult UpdateById(FeedbackDto feedbackDto, Guid id) => Ok(_feedbackService.UpdateById(feedbackDto, id));

        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_feedbackService.DeleteById(id));
    }
}