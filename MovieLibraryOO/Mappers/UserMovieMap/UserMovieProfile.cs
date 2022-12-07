using AutoMapper;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibraryOO.Mappers.UserMovieMap
{
    public class UserMovieProfile : Profile
    {
        public UserMovieProfile()
        {
            CreateMap<UserMovie, UserMovieDto>();
            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }
}
