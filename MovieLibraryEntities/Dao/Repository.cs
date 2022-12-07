using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public class Repository : IRepository, IDisposable
    {
        private readonly IDbContextFactory<MovieContext> _contextFactory;
        private readonly MovieContext _context;

        public Repository(IDbContextFactory<MovieContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<Movie> GetAll()
        {
            //For whatever reason, right after using db.Movies.Update(updateMovie);
            //Using _context ends not updating, or, quite strangly, fall behind, and apply updates only after other updates occur
            //Changing GetAll and Search to use var db seems to solve this issue
            using (var db = new MovieContext())
            {
                return db.Movies.ToList();
            }
        }

        public IEnumerable<Movie> Search(string searchString)
        {
            using (var db = new MovieContext())
            {
                var allMovies = db.Movies;
                var listOfMovies = allMovies.ToList();
                var temp = listOfMovies.Where(x => x.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));

                return temp;
            }
        }

        public IEnumerable<User> GetAllUser()
        {
            using (var db = new MovieContext())
            {
                return db.Users.ToList();
            }
        }

        public IEnumerable<Occupation> GetAllOccupation()
        {
            using (var db = new MovieContext())
            {
                return db.Occupations.ToList();
            }
        }
    }
}
