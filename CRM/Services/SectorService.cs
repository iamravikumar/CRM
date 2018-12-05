using CRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Services
{
    public class SectorService
    {
        private readonly ApplicationDbContext _context;

        public SectorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsSectorUsed(int id)
        {
            var firms = _context.Firms.Where(t => t.SectorID == id).ToList().Count();

            if (firms == 0)
                return false;
            else
                return true;
        }
    }
}
