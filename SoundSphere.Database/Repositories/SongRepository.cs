using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Database.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly SoundSphereDbContext _context;

        public SongRepository(SoundSphereDbContext context) => _context = context;

        public IList<Song> GetAll() => _context.Songs
            .Include(song => song.Album)
            .Include(song => song.Artists)
            .Include(song => song.SimilarSongs)
            .Where(song => song.DeletedAt == null)
            .OrderBy(song => song.CreatedAt)
            .ToList();

        public Song GetById(Guid id) => _context.Songs
            .Include(song => song.Album)
            .Include(song => song.Artists)
            .Include(song => song.SimilarSongs)
            .Where(song => song.DeletedAt == null)
            .SingleOrDefault(song => song.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(SongNotFound, id));

        public Song Add(Song song)
        {
            if (song.Id == Guid.Empty) song.Id = Guid.NewGuid();
            song.CreatedAt = DateTime.Now;
            _context.Songs.Add(song);
            _context.SaveChanges();
            return song;
        }

        public Song UpdateById(Song song, Guid id)
        {
            Song songToUpdate = GetById(id);
            songToUpdate.Title = song.Title;
            songToUpdate.ImageUrl = song.ImageUrl;
            songToUpdate.Genre = song.Genre;
            songToUpdate.ReleaseDate = song.ReleaseDate;
            songToUpdate.DurationSeconds = song.DurationSeconds;
            songToUpdate.Album = song.Album;
            songToUpdate.Artists = song.Artists;
            songToUpdate.SimilarSongs = song.SimilarSongs;
            if (_context.Entry(songToUpdate).State == EntityState.Modified)
                songToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return songToUpdate;
        }

        public Song DeleteById(Guid id)
        {
            Song songToDelete = GetById(id);
            songToDelete.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            return songToDelete;
        }

        public void LinkSongToAlbum(Song song)
        {
            if (_context.Albums.Find(song.Album.Id) is Album existingAlbum)
                song.Album = _context.Attach(existingAlbum).Entity;
        }

        public void LinkSongToArtists(Song song) => song.Artists = song.Artists
            .Select(artist => _context.Artists.Find(artist.Id))
            .Where(artist => artist != null)
            .Select(artist => _context.Attach(artist!).Entity)
            .ToList();

        public void AddSongLink(Song song) => song.SimilarSongs = song.SimilarSongs
            .Select(similarSong => _context.Songs.Find(similarSong.SimilarSongId))
            .Where(similarSong => similarSong != null)
            .Select(similarSong => new SongLink { Song = song, SimilarSong = similarSong })
            .ToList();

        public void AddUserSong(Song song) => _context.UsersSongs.AddRange(_context.Users
            .Select(user => new UserSong { User = user, Song = song, PlayCount = 0 })
            .ToList());
    }
}