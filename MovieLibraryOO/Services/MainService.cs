using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Dao;
using MovieLibraryOO.Dto;
using MovieLibraryOO.Mappers;
using System;
using System.Linq;

namespace MovieLibraryOO.Services
{
    public class MainService : IMainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IMovieMapper _movieMapper;
        private readonly IRepository _repository;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IRatingService _ratingService;

        public MainService(ILogger<MainService> logger, IMovieMapper movieMapper, IRepository repository, IFileService fileService, IUserService userService, IRatingService ratingService)
        {
            _logger = logger;
            _movieMapper = movieMapper;
            _repository = repository;
            _fileService = fileService;
            _userService = userService;
            _ratingService = ratingService;
        }

        public void Invoke()
        {
            var menu = new Menu();

            Menu.MenuOptions menuChoice;
            do
            {
                menuChoice = menu.ChooseAction();

                switch (menuChoice)
                {
                    case Menu.MenuOptions.ListFromDb:
                        _logger.LogInformation("Listing movies from database");
                        var allMovies = _repository.GetAll();
                        var movies = _movieMapper.Map(allMovies);
                        ConsoleTable.From<MovieDto>(movies).Write();
                        break;
                    case Menu.MenuOptions.ListFromFile:
                        _fileService.Read();
                        _fileService.Display();
                        break;
                    case Menu.MenuOptions.Add:
                        _logger.LogInformation("Adding a new movie");

                        //Gets the Movie Title
                        var movieTitle = menu.GetUserResponse("Enter the Movie's", "Title:", "green");

                        //(Redundent thanks to the use of menu) Ensures the Title isn't null or only spaces
                        if (string.IsNullOrWhiteSpace(movieTitle))
                        {
                            Console.WriteLine("The Title cannot be blank!");
                        }
                        else
                        {
                            using (var db = new MovieContext())
                            {
                                //Ensures the given title doesn't already exist in the database
                                if (db.Movies.FirstOrDefault(x => x.Title == movieTitle) != null)
                                {
                                    Console.WriteLine("That movie already exists in this database!");
                                }
                                else
                                {
                                    //Gets the Movie's Release Date
                                    var movieRelease = menu.GetUserResponse($"Enter {movieTitle}'s", "Release Date:", "green");

                                    //Ensures the date given is actually a date
                                    try
                                    {
                                        var movieReleaseDate = DateTime.Parse(movieRelease);

                                        //Creates and adds the movie to the database
                                        var movie = new Movie()
                                        {
                                            Title = movieTitle,
                                            ReleaseDate = movieReleaseDate
                                        };
                                        db.Movies.Add(movie);
                                        db.SaveChanges();

                                        //Confirms that the addition of the movie in the database
                                        var newMovie = db.Movies.FirstOrDefault(x => x.Title == movieTitle);
                                        Console.WriteLine($"({newMovie.Id}) {newMovie.Title}, released: {newMovie.ReleaseDate}");
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.WriteLine("The date entered isn't valid!");
                                    }
                                }
                            }
                        }
                        break;
                    case Menu.MenuOptions.Update:
                        _logger.LogInformation("Updating an existing movie");

                        //Gets the Title of the Movie getting UPDATED
                        var oldMovieTitle = menu.GetUserResponse("Enter the Title of the Movie to", "Update:", "yellow");
                        
                        using (var db = new MovieContext())
                        {
                            //Ensures the given title exists in the database
                            var updateMovie = db.Movies.FirstOrDefault(x => x.Title == oldMovieTitle);
                            if (updateMovie != null)
                            {
                                //Gets the Movie's NEW Title
                                var newMovieTitle = menu.GetUserResponse("Enter the Movie's", "New/Correct Title:", "green");

                                //(Redundent thanks to the use of menu) Ensures the Title isn't null or only spaces
                                if (string.IsNullOrWhiteSpace(newMovieTitle))
                                {
                                    Console.WriteLine("The Title cannot be blank!");
                                } 
                                //Ensures the given title doesn't already exist in the database and isn't keeping the same Title
                                else if (db.Movies.FirstOrDefault(x => x.Title == newMovieTitle) != null && updateMovie.Title != newMovieTitle) 
                                {
                                    Console.WriteLine("That movie already exists in this database under a different id!");
                                }
                                else
                                {
                                    //Gets the Movie's NEW Release Date
                                    var newMovieRelease = menu.GetUserResponse($"Enter {newMovieTitle}'s", "New/Correct Release Date:", "green");

                                    //Ensures the date given is actually a date
                                    try
                                    {
                                        var newMovieReleaseDate = DateTime.Parse(newMovieRelease);
                            
                                        Console.WriteLine($"({updateMovie.Id}) {updateMovie.Title}, released: {updateMovie.ReleaseDate}");

                                        //Updates the movie in the database
                                        updateMovie.Title = newMovieTitle;
                                        updateMovie.ReleaseDate = newMovieReleaseDate;
                                        db.Movies.Update(updateMovie);
                                        db.SaveChanges();

                                        //Confirms that the movie was updated in the database
                                        Console.WriteLine("The details above, have now been replaced with the following: ");
                                        Console.WriteLine($"({updateMovie.Id}) {updateMovie.Title}, released: {updateMovie.ReleaseDate}");
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.WriteLine("The date entered isn't valid!");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("That movie doesn't exists in this database!");
                                Console.WriteLine("Check your spelling and try again");
                            }
                        }
                        break;
                    case Menu.MenuOptions.Delete:
                        _logger.LogInformation("Deleting a movie");

                        //Gets the Title of the Movie getting DELETED
                        var deletingMovieTitle = menu.GetUserResponse("Enter The Title of the Movie to", "DELETE:", "red");

                        using (var db = new MovieContext())
                        {
                            //Verifies that the Movie exists in the database
                            var deleteMovie = db.Movies.FirstOrDefault(x => x.Title == deletingMovieTitle);
                            if (deleteMovie != null)
                            {
                                //Displays the details of the movie thats about to be DELETED from the database
                                Console.WriteLine($"({deleteMovie.Id}) {deleteMovie.Title}, released: {deleteMovie.ReleaseDate}");

                                //Deletes the movie from the database
                                db.Movies.Remove(deleteMovie);
                                db.SaveChanges();

                                //Confirms that the program at least attempted to the delete the given movie and didn't throw an error
                                Console.WriteLine("This Movie has been deleted");
                            }
                            else
                            {
                                Console.WriteLine($"{deletingMovieTitle} doesn't exist in the database");
                                Console.WriteLine("Check your spelling and try again");
                            }
                        }
                        break;
                    case Menu.MenuOptions.Search:
                        _logger.LogInformation("Searching for a movie");
                        var userSearchTerm = menu.GetUserResponse("Enter your", "search string:", "green");
                        var searchedMovies = _repository.Search(userSearchTerm);
                        var table = new ConsoleTable("Id", "Title", "ReleaseDate", "Genres");

                        //A table must be constructed which retrieves the genres of that result from the search
                        //Try to be specific, as the large number of loops needed to achieve this don't play well with the program's performance
                        using (var db = new MovieContext())
                        {
                            var theMovieGenres = db.MovieGenres;
                            var listOfMovieGenre = theMovieGenres.ToList();

                            foreach (var movie in searchedMovies)
                            {
                                int genreCount = 0;
                                var genreString = "";
                                var genreList = listOfMovieGenre.Where(x => x.MovieId.Equals(movie.Id));
                                foreach (var movieGenre in genreList)
                                {
                                    var genre = db.Genres.FirstOrDefault(x => x.Id == movieGenre.GenreId).Name;

                                    if (genreCount == 0)
                                    {
                                        genreString = $"{genre}";
                                    } else { 
                                        genreString = $"{genreString}, {genre}";
                                    }
                                    genreCount++;
                                }

                                table.AddRow(movie.Id, movie.Title, movie.ReleaseDate, genreString);
                            }
                        }

                        Console.WriteLine(table);

                        //movies = _movieMapper.Map(searchedMovies);
                        //ConsoleTable.From<MovieDto>(movies).Write();
                        break;
                    case Menu.MenuOptions.ToUsers:
                        _userService.Invoke();
                        break;
                    case Menu.MenuOptions.ToRatings:
                        _ratingService.Invoke();
                        break;
                }
            }
            while (menuChoice != Menu.MenuOptions.Exit);

            menu.Exit();


            Console.WriteLine("\nThanks for using the Movie Library!");

        }
    }
}