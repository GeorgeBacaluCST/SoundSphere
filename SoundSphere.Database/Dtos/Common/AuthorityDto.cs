using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class AuthorityDto
    {
        [Required(ErrorMessage = "ID is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public AuthorityType Type { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}