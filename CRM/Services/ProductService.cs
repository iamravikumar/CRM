using CRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsProductUsed(int id)
        {
            var services = _context.Services.Where(t => t.ProductID == id).ToList().Count();

            if (services == 0)
                return false;
            else
                return true;
        }
    }
}
