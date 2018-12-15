using CRM.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Services
{
    public class PaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsDone(int id)
        {
            var payments = _context.Payments.Where(p => p.ServiceID == id).Select(p => p.IsDone).ToList();

            bool result = false;

            foreach (var item in payments)
            {
                if (item == true)
                    result = true;
                else
                    result = false;
            }

            return result;
        }

        public bool IsPaid(int id)
        {
            var payments = _context.Payments.Where(p => p.ServiceID == id).Select(p => p.IsDone).ToList();

            bool result = false;

            foreach (var item in payments)
            {
                if (item == true)
                    result = true;
            }

            return result;
        }
    }
}
