using AutoMapper;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibraryOO.Mappers.UserMap
{
    internal class UserMapper : IUserMapper
    {
        private readonly IMapper _mapper;

        public UserMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<UserDto> Map(IEnumerable<User> users)
        {
            IEnumerable<UserDto> dto = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
            return dto;
        }

        public UserDto Map(User user)
        {
            UserDto dto = _mapper.Map<User, UserDto>(user);
            return dto;
        }
    }
}
