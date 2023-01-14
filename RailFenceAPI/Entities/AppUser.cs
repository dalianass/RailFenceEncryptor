using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailFenceAPI.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string UserSurname { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }

        public ICollection<Sifrovanje> Sifrovanja { get; set; }


        public ICollection<Desifrovanje> Desifrovanja { get; set; }
    }
}
