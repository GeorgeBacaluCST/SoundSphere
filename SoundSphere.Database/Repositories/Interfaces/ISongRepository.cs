﻿using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface ISongRepository
    {
        IList<Song> GetAll();

        Song GetById(Guid id);

        Song Add(Song song);

        Song UpdateById(Song song, Guid id);

        Song DeleteById(Guid id);

        void LinkSongToAlbum(Song song);

        void LinkSongToArtists(Song song);

        void AddSongLink(Song song);

        void AddUserSong(Song song);
    }
}