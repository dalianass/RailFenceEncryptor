using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailFenceAPI.DTOs
{
    public class SvaSifrovanjaDto
    {

        public string RecZaSifrovanje { get; set; }

        public int Dubina { get; set; }

        public string SifrovanaRec { get; set; }

        public int AppUserId { get; set; }

        public UserDto AppUser { get; set; }
    }
}
