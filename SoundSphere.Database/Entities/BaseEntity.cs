﻿namespace SoundSphere.Database.Entities
{
    public class BaseEntity
    {
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? DeletedAt { get; set; }
    }
}