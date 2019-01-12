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
        public GenderType GenderType { get; set; }
        public string StartAge { get; set; }
        public string EndAge { get; set; }
        public string sortByAsc { get; set; }
        public string sortByDsc { get; set; }
    }

}
