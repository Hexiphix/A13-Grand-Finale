using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibraryOO.Dto
{
    public class UserMovieDto
    {
        public long Id { get; set; }
        public long Rating { get; set; }
        public DateTime RatedAt { get; set; }
        public long UserId { get; set; }
        public long MovieId { get; set; }
    }
}
