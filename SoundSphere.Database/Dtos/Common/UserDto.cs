﻿using SoundSphere.Database.Attributes;
using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Common
{
    public class UserDto : BaseEntity
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(75, ErrorMessage = "Name can't be longer than 75 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^(00|\+?40|0)(7\d{2}|\d{2}[13]|[2-37]\d|8[02-9]|9[0-2])\s?\d{3}\s?\d{3}$", ErrorMessage = "Invalid mobile format")]
        public string Mobile { get; set; } = null!; // Mobile format: 00/+40/0 + mobile prefix + optional space + 3 digits (first part) + optional space + 3 digits (second part)

        [Required(ErrorMessage = "Address is required")]
        [StringLength(150, ErrorMessage = "Address can't be longer than 150 characters")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Birthday is required")]
        [Date(ErrorMessage = "Birthday can't be in the future")]
        public DateOnly Birthday { get; set; }

        [Required(ErrorMessage = "Avatar is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string Avatar { get; set; } = null!;

        [Required(ErrorMessage = "Role ID is required")]
        public Guid RoleId { get; set; }

        [MaxLength(4, ErrorMessage = "There can't be more than 4 authorities")]
        public IList<Guid> AuthoritiesIds { get; set; } = null!;
    }
}