using CRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Services
{
    public class ScheduleService
    {
        private readonly ApplicationDbContext _context;

        public ScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int DailyScheduleCounts(int id)
        {
            return _context.Schedules.Where(t => t.TeamID == id).Where(t => t.StartedAt.Date == DateTime.Today.Date).ToList().Count();
        }
    }
}
