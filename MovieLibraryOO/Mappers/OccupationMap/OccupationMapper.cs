using AutoMapper;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Dto;
using MovieLibraryOO.Mappers.UserMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibraryOO.Mappers.OccupationMap
{
    public class OccupationMapper : IOccupationMapper
    {
        private readonly IMapper _mapper;

        public OccupationMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<OccupationDto> Map(IEnumerable<Occupation> occupations)
        {
            IEnumerable<OccupationDto> dto = _mapper.Map<IEnumerable<Occupation>, IEnumerable<OccupationDto>>(occupations);
            return dto;
        }

        public OccupationDto Map(Occupation occupation)
        {
            OccupationDto dto = _mapper.Map<Occupation, OccupationDto>(occupation);
            return dto;
        }
    }
}
