using AutoMapper;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Dto;
using System.Collections.Generic;

namespace MovieLibraryOO.Mappers.UserMovieMap
{
    public class UserMovieMapper : IUserMovieMapper
    {
        private readonly IMapper _mapper;

        public UserMovieMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<UserMovieDto> Map(IEnumerable<UserMovie> userMovies)
        {
            IEnumerable<UserMovieDto> dto = _mapper.Map<IEnumerable<UserMovie>, IEnumerable<UserMovieDto>>(userMovies);
            return dto;
        }

        public UserMovieDto Map(UserMovie userMovie)
        {
            UserMovieDto dto = _mapper.Map<UserMovie, UserMovieDto>(userMovie);
            return dto;
        }
    }
}
