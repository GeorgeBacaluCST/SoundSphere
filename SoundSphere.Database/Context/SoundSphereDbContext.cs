using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Context
{
    public class SoundSphereDbContext : DbContext
    {
        public SoundSphereDbContext(DbContextOptions<SoundSphereDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}