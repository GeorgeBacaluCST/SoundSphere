using SoundSphere.Database.Attributes;
using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class AlbumDto : BaseEntity
    {
        [Required(ErrorMessage = "ID is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(75, ErrorMessage = "Title can't be longer than 75 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "ImageURL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Release date is required")]
        [Date(ErrorMessage = "Release date can't be in the future")]
        public DateOnly ReleaseDate { get; set; }

        [MaxLength(15, ErrorMessage = "There can't be more than 15 similar albums")]
        public List<Guid> SimilarAlbumsIds { get; set; } = new()!;
    }
}