using CRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<Firm> Firms { get; set; }
        public DbSet<Programme> Programmes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
    }
}
