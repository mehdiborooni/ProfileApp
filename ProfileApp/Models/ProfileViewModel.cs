using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileApp.Models
{
    public class ProfileViewModel
    {
        public IEnumerable<ProfileApp.Models.Profile> Users { get; set; }
        public IsActiveType IsActive { get; set; }
        public string Term { get; set; }
    }

}
