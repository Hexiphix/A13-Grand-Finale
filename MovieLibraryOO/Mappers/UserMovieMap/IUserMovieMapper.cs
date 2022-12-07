using MovieLibraryEntities.Models;
using MovieLibraryOO.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibraryOO.Mappers.UserMovieMap
{
    public interface IUserMovieMapper
    {
        IEnumerable<UserMovieDto> Map(IEnumerable<UserMovie> userMovies);
    }
}
