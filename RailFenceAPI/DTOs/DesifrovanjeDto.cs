using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailFenceAPI.DTOs
{
    public class DesifrovanjeDto
    {
        //public int Id { get; set; }

        public string RecZaDesifrovanje { get; set; }

        public int Dubina { get; set; }

        public string DesifrovanaRec { get; set; }

        public int AppUserId { get; set; }

        //public UserDto AppUser { get; set; }
    }
}
