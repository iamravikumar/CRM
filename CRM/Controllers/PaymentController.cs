using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
                return NotFound();

            Payment payment = await _context.Payments.FindAsync(id);

            if (payment == null)
                return NotFound();

            return View();
        }
    }
}