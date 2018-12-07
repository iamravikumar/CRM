using CRM.Data;
using CRM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Services
{
    public class TeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int MemberCounts(int id)
        {
            return _context.TeamMembers.Where(t => t.TeamID == id).Where(t => t.IsActive == true).ToList().Count();
        }

        public int ApplicatanCounts(int id)
        {
            return _context.TeamMembers.Where(t => t.TeamID == id).Where(t => t.IsActive == false).ToList().Count();
        }

        public int FirmCounts(int id)
        {
            return _context.Firms.Where(t => t.TeamID == id).ToList().Count();
        }

        public int PersonnelCounts(int id)
        {
            return _context.Personnels.Where(t => t.TeamID == id).ToList().Count();
        }

        public TeamMember TeamFounder(int id)
        {
            return _context.TeamMembers.Where(t => t.TeamID == id).Include(t => t.User).OrderBy(t => t.CreatedAt).Take(1).FirstOrDefault();
        }
    }
}
