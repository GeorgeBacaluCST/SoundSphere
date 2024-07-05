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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService) => _feedbackService = feedbackService;

        /// <summary>Get all feedbacks</summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet] public IActionResult GetAll() => Ok(_feedbackService.GetAll());

        /// <summary>Get feedback by ID</summary>
        /// <param name="id">Feedback fetching ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] public IActionResult GetById(Guid id) => Ok(_feedbackService.GetById(id));

        /// <summary>Add feedback</summary>
        /// <param name="feedbackDto">FeedbackDTO to add</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost] public IActionResult Add(FeedbackDto feedbackDto)
        {
            FeedbackDto addedFeedbackDto = _feedbackService.Add(feedbackDto);
            return CreatedAtAction(nameof(GetById), new { addedFeedbackDto.Id }, addedFeedbackDto);
        }

        /// <summary>Update feedback by ID</summary>
        /// <param name="feedbackDto">FeedbackDTO to update</param>
        /// <param name="id">Feedback updating ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")] public IActionResult UpdateById(FeedbackDto feedbackDto, Guid id) => Ok(_feedbackService.UpdateById(feedbackDto, id));

        /// <summary>Soft delete feedback by ID</summary>
        /// <param name="id">Feedback deleting ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")] public IActionResult DeleteById(Guid id) => Ok(_feedbackService.DeleteById(id));
    }
}