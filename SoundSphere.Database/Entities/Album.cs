namespace SoundSphere.Database.Entities
{
    public class Album : BaseEntity
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = null!;
        
        public string ImageUrl { get; set; } = null!;
        
        public DateOnly ReleaseDate { get; set; }
        
        public IList<AlbumLink> SimilarAlbums { get; set; } = new List<AlbumLink>();
    }
}