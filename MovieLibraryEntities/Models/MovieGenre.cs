namespace MovieLibraryEntities.Models
{
    public class MovieGenre
    {
        public int Id { get; set; }
        public long MovieId { get; set; }
        public long GenreId { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
