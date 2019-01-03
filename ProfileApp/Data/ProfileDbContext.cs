using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ProfileApp.Data
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options) { }
        public DbSet<Profile> Profiles { get; set; }
    }
}

