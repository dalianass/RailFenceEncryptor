using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailFenceAPI.Entities
{
    public class Sifrovanje
    {
        public int Id { get; set; }

        public string RecZaSifrovanje { get; set; }

        public int Dubina { get; set; }

        public string SifrovanaRec { get; set; }

        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
