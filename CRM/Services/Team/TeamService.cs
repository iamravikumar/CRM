using CRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Services.Team
{
    public class TeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int MembersCount(int id)
        {
            return _context.TeamMembers.Where(t => t.TeamID == id).Where(t => t.IsActive == true).ToList().Count();
        }
    }
}
