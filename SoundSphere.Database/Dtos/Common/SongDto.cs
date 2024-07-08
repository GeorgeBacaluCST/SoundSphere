using SoundSphere.Database.Attributes;
using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class SongDto : BaseEntity
    {
        [Required(ErrorMessage = "ID is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(75, ErrorMessage = "Title can't be longer than 75 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "ImageURL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Genre is required")]
        public Genre Genre { get; set; }

        [Required(ErrorMessage = "Release date is required")]
        [Date(ErrorMessage = "Release date can't be in the future")]
        public DateOnly ReleaseDate { get; set; }

        [Range(0, 300, ErrorMessage = "Song can't be longer than 5 minutes")]
        public int DurationSeconds { get; set; }

        [Required(ErrorMessage = "Album ID is required")]
        public Guid AlbumId { get; set; }

        [MaxLength(15, ErrorMessage = "There can't be more than 15 artists")]
        public IList<Guid> ArtistsIds { get; set; } = new List<Guid>();

        [MaxLength(15, ErrorMessage = "There can't be more than 15 similar songs")]
        public IList<Guid> SimilarSongsIds { get; set; } = new List<Guid>();
    }
}