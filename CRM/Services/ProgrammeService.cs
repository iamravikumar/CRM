using CRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Services
{
    public class ProgrammeService
    {
        private readonly ApplicationDbContext _context;

        public ProgrammeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsProgrammeUsed(int id)
        {
            var schedules = _context.Schedules.Where(s => s.ProgrammeID == id).ToList().Count();

            if (schedules == 0)
                return false;
            else
                return true;
        }
    }
}
