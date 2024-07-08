using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Database.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly SoundSphereDbContext _context;

        public PlaylistRepository(SoundSphereDbContext context) => _context = context;

        public IList<Playlist> GetAll() => _context.Playlists
            .Include(playlist => playlist.User)
            .Include(playlist => playlist.Songs)
            .Where(playlist => playlist.DeletedAt == null)
            .OrderBy(playlist => playlist.CreatedAt)
            .ToList();

        public Playlist GetById(Guid id) => _context.Playlists
            .Include(playlist => playlist.User)
            .Include(playlist => playlist.Songs)
            .Where(playlist => playlist.DeletedAt == null)
            .SingleOrDefault(playlist => playlist.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(PlaylistNotFound, id));

        public Playlist Add(Playlist playlist)
        {
            if (playlist.Id == Guid.Empty) playlist.Id = Guid.NewGuid();
            playlist.CreatedAt = DateTime.Now;
            _context.Playlists.Add(playlist);
            _context.SaveChanges();
            return playlist;
        }

        public Playlist UpdateById(Playlist playlist, Guid id)
        {
            Playlist playlistToUpdate = GetById(id);
            playlistToUpdate.Title = playlist.Title;
            playlistToUpdate.Songs = playlist.Songs;
            if (_context.Entry(playlistToUpdate).State == EntityState.Modified)
                playlistToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return playlistToUpdate;
        }

        public Playlist DeleteById(Guid id)
        {
            Playlist playlistToDelete = GetById(id);
            playlistToDelete.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            return playlistToDelete;
        }

        public void LinkPlaylistToUser(Playlist playlist)
        {
            if (_context.Users.Find(playlist.User.Id) is User existingUser)
                playlist.User = _context.Attach(existingUser).Entity;
        }
    }
}