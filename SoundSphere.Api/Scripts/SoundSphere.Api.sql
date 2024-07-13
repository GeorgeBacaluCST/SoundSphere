INSERT INTO [dbo].[Roles] ([Id], [Type], [CreatedAt]) VALUES 
('deaf35ba-fe71-4c21-8a3c-d8e5b32a06fe', 'Admin', CAST('2024-07-01T00:00:00.0000000' AS DateTime2)),
('2fc8f207-5af0-402f-84d0-e1c7fa7336a6', 'Moderator', CAST('2024-07-01T00:00:00.0000001' AS DateTime2)),
('61ee6dda-e18a-4eb9-a736-3f95ba5537f7', 'Listener',CAST('2024-07-01T00:00:00.0000002' AS DateTime2));

INSERT INTO [dbo].[Authorities] ([Id], [Type], [CreatedAt]) VALUES 
('75e924c3-34e7-46ef-b521-7331e36caadd', 'Create', CAST('2024-07-01T00:00:00.0000000' AS DateTime2)),
('362b20cf-3636-49ed-9489-d2700339efce', 'Read', CAST('2024-07-01T00:00:00.0000001' AS DateTime2)),
('3cc47e2d-b14e-472f-9868-fbb90b15f18e', 'Update', CAST('2024-07-01T00:00:00.0000002' AS DateTime2)),
('59525baa-6eaa-42d8-b213-9094af0d604b', 'Delete', CAST('2024-07-01T00:00:00.0000003' AS DateTime2));

INSERT INTO [dbo].[Albums] ([Id], [Title], [ImageUrl], [ReleaseDate], [CreatedAt], [UpdatedAt], [DeletedAt]) VALUES
('6ee76a77-2be4-42e3-8417-e60d282cffcb', 'album_title1', 'https://album-imageurl1.jpg', CAST('2020-01-01' AS Date), CAST('2024-06-01T00:00:00.0000000' AS DateTime2), NULL, NULL),
('b58f5f3f-d5e8-49f3-8b12-cfe33f762b4f', 'album_title2', 'https://album-imageurl2.jpg', CAST('2020-01-02' AS Date), CAST('2024-06-02T00:00:00.0000000' AS DateTime2), NULL, NULL);

INSERT INTO [dbo].[AlbumLinks] ([AlbumId], [SimilarAlbumId]) VALUES ('6ee76a77-2be4-42e3-8417-e60d282cffcb', 'b58f5f3f-d5e8-49f3-8b12-cfe33f762b4f');

INSERT INTO [dbo].[Artists] ([Id], [Name], [ImageUrl], [Bio], [CreatedAt], [UpdatedAt], [DeletedAt]) VALUES 
('4e75ecdd-aafe-4c35-836b-1b83fc7b8f88', 'artist_name1', 'https://artist-imageurl1.jpg', 'artist_bio1', CAST('2024-06-01T00:00:00.0000000' AS DateTime2), NULL, NULL),
('8c301aa9-6d56-4c06-b1f2-9b9956979345', 'artist_name2', 'https://artist-imageurl2.jpg', 'artist_bio2', CAST('2024-06-02T00:00:00.0000000' AS DateTime2), NULL, NULL);

INSERT INTO [dbo].[ArtistLinks] ([ArtistId], [SimilarArtistId]) VALUES ('4e75ecdd-aafe-4c35-836b-1b83fc7b8f88', '8c301aa9-6d56-4c06-b1f2-9b9956979345');

INSERT INTO [dbo].[Songs] ([Id], [Title], [ImageUrl], [Genre], [ReleaseDate], [DurationSeconds], [AlbumId], [CreatedAt], [UpdatedAt], [DeletedAt]) VALUES 
('64f534f8-f2d4-4402-95a3-54de48b678a8', 'song_title1', 'https://song-imageurl1.jpg', 'Pop', CAST('2020-01-01' AS Date), 180, '6ee76a77-2be4-42e3-8417-e60d282cffcb', CAST('2024-06-01T00:00:00.0000000' AS DateTime2), NULL, NULL),
('278cfa5a-6f44-420e-9930-07da6c43a6ad', 'song_title2', 'https://song-imageurl2.jpg', 'Rock', CAST('2020-01-02' AS Date), 185, '6ee76a77-2be4-42e3-8417-e60d282cffcb', CAST('2024-06-02T00:00:00.0000000' AS DateTime2), NULL, NULL),
('7ef7351b-912e-4a64-ba6d-cfdfcb7d56af', 'song_title3', 'https://song-imageurl3.jpg', 'Rnb', CAST('2020-01-03' AS Date), 190, 'b58f5f3f-d5e8-49f3-8b12-cfe33f762b4f', CAST('2024-06-03T00:00:00.0000000' AS DateTime2), NULL, NULL),
('03b3fb9f-38af-4074-8ab5-b9644ab44397', 'song_title4', 'https://song-imageurl4.jpg', 'HipHop', CAST('2020-01-04' AS Date), 195, 'b58f5f3f-d5e8-49f3-8b12-cfe33f762b4f', CAST('2024-06-04T00:00:00.0000000' AS DateTime2), NULL, NULL);

INSERT INTO [dbo].[SongLinks] ([SongId], [SimilarSongId]) VALUES
('64f534f8-f2d4-4402-95a3-54de48b678a8', '278cfa5a-6f44-420e-9930-07da6c43a6ad'),
('278cfa5a-6f44-420e-9930-07da6c43a6ad', '7ef7351b-912e-4a64-ba6d-cfdfcb7d56af'),
('7ef7351b-912e-4a64-ba6d-cfdfcb7d56af', '03b3fb9f-38af-4074-8ab5-b9644ab44397');

INSERT INTO [dbo].[ArtistSong] ([ArtistsId], [SongsId]) VALUES
('4e75ecdd-aafe-4c35-836b-1b83fc7b8f88', '64f534f8-f2d4-4402-95a3-54de48b678a8'),
('4e75ecdd-aafe-4c35-836b-1b83fc7b8f88', '278cfa5a-6f44-420e-9930-07da6c43a6ad'),
('8c301aa9-6d56-4c06-b1f2-9b9956979345', '7ef7351b-912e-4a64-ba6d-cfdfcb7d56af'),
('8c301aa9-6d56-4c06-b1f2-9b9956979345', '03b3fb9f-38af-4074-8ab5-b9644ab44397');

INSERT [dbo].[Users] ([Id], [Name], [Email], [Password], [Mobile], [Address], [Birthday], [Avatar], [RoleId], [CreatedAt], [UpdatedAt], [DeletedAt]) VALUES
('0a9e546f-38b4-4dbf-a482-24a82169890e', 'user_name1', 'user_email1@email.com', '#User1_password!', '+40700000000', 'user_address1', CAST('2000-01-01' AS Date), 'https://user1-avatar.jpg', 'deaf35ba-fe71-4c21-8a3c-d8e5b32a06fe', CAST('2024-07-01T00:00:00.0000000' AS DateTime2), NULL, NULL),
('7eb88892-549b-4cae-90be-c52088354643', 'user_name2', 'user_email2@email.com', '#User2_password!', '+40700000001', 'user_address2', CAST('2000-01-02' AS Date), 'https://user2-avatar.jpg', '2fc8f207-5af0-402f-84d0-e1c7fa7336a6', CAST('2024-07-02T00:00:00.0000000' AS DateTime2), NULL, NULL);

INSERT INTO [dbo].[AuthorityUser] ([AuthoritiesId], [UsersId]) VALUES
('75e924c3-34e7-46ef-b521-7331e36caadd', '0a9e546f-38b4-4dbf-a482-24a82169890e'),
('362b20cf-3636-49ed-9489-d2700339efce', '0a9e546f-38b4-4dbf-a482-24a82169890e'),
('3cc47e2d-b14e-472f-9868-fbb90b15f18e', '0a9e546f-38b4-4dbf-a482-24a82169890e'),
('59525baa-6eaa-42d8-b213-9094af0d604b', '0a9e546f-38b4-4dbf-a482-24a82169890e'),
('75e924c3-34e7-46ef-b521-7331e36caadd', '7eb88892-549b-4cae-90be-c52088354643'),
('362b20cf-3636-49ed-9489-d2700339efce', '7eb88892-549b-4cae-90be-c52088354643'),
('3cc47e2d-b14e-472f-9868-fbb90b15f18e', '7eb88892-549b-4cae-90be-c52088354643');

INSERT [dbo].[UsersArtists] ([UserId], [ArtistId], [IsFollowing]) VALUES
('0a9e546f-38b4-4dbf-a482-24a82169890e', '4e75ecdd-aafe-4c35-836b-1b83fc7b8f88', 0),
('0a9e546f-38b4-4dbf-a482-24a82169890e', '8c301aa9-6d56-4c06-b1f2-9b9956979345', 0),
('7eb88892-549b-4cae-90be-c52088354643', '4e75ecdd-aafe-4c35-836b-1b83fc7b8f88', 0),
('7eb88892-549b-4cae-90be-c52088354643', '8c301aa9-6d56-4c06-b1f2-9b9956979345', 0);

INSERT [dbo].[UsersSongs] ([UserId], [SongId], [PlayCount]) VALUES
('0a9e546f-38b4-4dbf-a482-24a82169890e', '64f534f8-f2d4-4402-95a3-54de48b678a8', 0),
('0a9e546f-38b4-4dbf-a482-24a82169890e', '278cfa5a-6f44-420e-9930-07da6c43a6ad', 0),
('0a9e546f-38b4-4dbf-a482-24a82169890e', '7ef7351b-912e-4a64-ba6d-cfdfcb7d56af', 0),
('0a9e546f-38b4-4dbf-a482-24a82169890e', '03b3fb9f-38af-4074-8ab5-b9644ab44397', 0),
('7eb88892-549b-4cae-90be-c52088354643', '64f534f8-f2d4-4402-95a3-54de48b678a8', 0),
('7eb88892-549b-4cae-90be-c52088354643', '278cfa5a-6f44-420e-9930-07da6c43a6ad', 0),
('7eb88892-549b-4cae-90be-c52088354643', '7ef7351b-912e-4a64-ba6d-cfdfcb7d56af', 0),
('7eb88892-549b-4cae-90be-c52088354643', '03b3fb9f-38af-4074-8ab5-b9644ab44397', 0);

INSERT INTO [dbo].[Feedbacks] ([Id], [UserId], [Type], [Message], [CreatedAt], [UpdatedAt], [DeletedAt]) VALUES 
('83061e8c-3403-441a-8be5-867ed1f4a86b', '0a9e546f-38b4-4dbf-a482-24a82169890e', 'Issue', 'feedback_message1', CAST('2024-07-01T00:00:00.0000000' AS DateTime2), NULL, NULL),
('bf823996-d2ce-4616-a6b2-f7347f05c6aa', '7eb88892-549b-4cae-90be-c52088354643', 'Optimization', 'feedback_message2', CAST('2024-07-02T00:00:00.0000000' AS DateTime2), NULL, NULL);

INSERT INTO [dbo].[Notifications] ([Id], [SenderId], [ReceiverId], [Type], [Message], [IsRead], [CreatedAt], [UpdatedAt], [DeletedAt]) VALUES
('7e221fa3-2c22-4573-bf21-cd1d6696b576', '0a9e546f-38b4-4dbf-a482-24a82169890e', '7eb88892-549b-4cae-90be-c52088354643', 'Music', 'notification_message1', 0, CAST('2024-07-01T00:00:00.0000000' AS DateTime2), NULL, NULL),
('1d23fa22-3455-407b-9371-c42d56001de7', '7eb88892-549b-4cae-90be-c52088354643', '0a9e546f-38b4-4dbf-a482-24a82169890e', 'Social', 'notification_message2', 0, CAST('2024-07-02T00:00:00.0000000' AS DateTime2), NULL, NULL);

INSERT INTO [dbo].[Playlists] ([Id], [Title], [UserId], [CreatedAt], [UpdatedAt], [DeletedAt]) VALUES 
('239d050b-b59c-47e0-9e1a-ab5faf6f903e', 'playlist_title1', '0a9e546f-38b4-4dbf-a482-24a82169890e', CAST('2024-07-01T00:00:00.0000000' AS DateTime2), NULL, NULL),
('67b394ad-aeba-4804-be29-71fc4ebd37c8', 'playlist_title2', '7eb88892-549b-4cae-90be-c52088354643', CAST('2024-07-02T00:00:00.0000000' AS DateTime2), NULL, NULL);

INSERT INTO [dbo].[PlaylistSong] ([PlaylistsId], [SongsId]) VALUES
('239d050b-b59c-47e0-9e1a-ab5faf6f903e', '64f534f8-f2d4-4402-95a3-54de48b678a8'),
('239d050b-b59c-47e0-9e1a-ab5faf6f903e', '278cfa5a-6f44-420e-9930-07da6c43a6ad'),
('67b394ad-aeba-4804-be29-71fc4ebd37c8', '7ef7351b-912e-4a64-ba6d-cfdfcb7d56af'),
('67b394ad-aeba-4804-be29-71fc4ebd37c8', '03b3fb9f-38af-4074-8ab5-b9644ab44397');