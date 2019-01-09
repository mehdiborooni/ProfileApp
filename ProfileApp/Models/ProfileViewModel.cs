using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileApp.Models
{
    public class ProfileViewModel
    {
        public IEnumerable<ProfileApp.Models.Profile> Users { get; set; }
        public IsActiveType IsActiveType { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
    }

}
