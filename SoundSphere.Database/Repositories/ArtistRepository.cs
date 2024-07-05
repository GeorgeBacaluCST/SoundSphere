﻿using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly SoundSphereDbContext _context;

        public ArtistRepository(SoundSphereDbContext context) => _context = context;

        public IList<Artist> GetAll() => _context.Artists
            .Include(artist => artist.SimilarArtists)
            .Where(artist => artist.DeletedAt == null)
            .OrderBy(artist => artist.CreatedAt)
            .ToList();

        public Artist GetById(Guid id) => _context.Artists
            .Include(artist => artist.SimilarArtists)
            .Where(artist => artist.DeletedAt == null)
            .SingleOrDefault(artist => artist.Id == id)
            ?? throw new ResourceNotFoundException($"Artist with id {id} not found");

        public Artist Add(Artist artist)
        {
            if (artist.Id == Guid.Empty) artist.Id = Guid.NewGuid();
            artist.CreatedAt = DateTime.Now;
            _context.Artists.Add(artist);
            _context.SaveChanges();
            return artist;
        }

        public Artist UpdateById(Artist artist, Guid id)
        {
            Artist artistToUpdate = GetById(id);
            artistToUpdate.Name = artist.Name;
            artistToUpdate.ImageUrl = artist.ImageUrl;
            artistToUpdate.Bio = artist.Bio;
            artistToUpdate.SimilarArtists = artist.SimilarArtists;
            if (_context.Entry(artistToUpdate).State == EntityState.Modified)
                artistToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return artistToUpdate;
        }

        public Artist DeleteById(Guid id)
        {
            Artist artistToDelete = GetById(id);
            artistToDelete.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            return artistToDelete;
        }

        public void AddArtistLink(Artist artist) => artist.SimilarArtists = artist.SimilarArtists
            .Select(similarArtist => _context.Artists.Find(similarArtist.SimilarArtistId))
            .Where(similarArtist => similarArtist != null)
            .Select(similarArtist => new ArtistLink { Artist = artist, SimilarArtist = similarArtist })
            .ToList();

        public void AddUserArtist(Artist artist) => _context.UsersArtists.AddRange(_context.Users
            .Select(user => new UserArtist { User = user, Artist = artist, IsFollowing = false })
            .ToList());
    }
}