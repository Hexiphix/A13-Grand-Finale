using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public interface IRepository
    {
        IEnumerable<Movie> GetAll();
        IEnumerable<Movie> Search(string searchString);
        IEnumerable<User> GetAllUser();
        IEnumerable<Occupation> GetAllOccupation();
    }
}
