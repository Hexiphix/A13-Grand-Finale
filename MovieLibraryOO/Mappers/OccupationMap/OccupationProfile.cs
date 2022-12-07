using AutoMapper;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibraryOO.Mappers.OccupationMap
{
    public class OccupationProfile : Profile
    {
        public OccupationProfile() 
        { 
            CreateMap<Occupation, OccupationDto>();
                // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }
}
