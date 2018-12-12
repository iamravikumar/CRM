using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using CRM.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    [Authorize(Roles = "Officer, Member")]
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public ServiceViewModel Model { get; set; }

        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);
            var services = await _context.Services
                                .Include(s => s.PaymentOption)
                                .Include(s => s.Product)
                                .Include(s => s.Personnel)
                                .ThenInclude(p => p.Firm)
                                .Where(t => t.TeamID == team.TeamID)
                                .ToListAsync();

            return View(services);
        }

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
                return NotFound();

            Personnel personnel = await _context.Personnels.FindAsync(id);

            if (personnel == null)
                return NotFound();

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);

            Model = new ServiceViewModel()
            {
                Products = await _context.Products.Where(p => p.TeamID == team.TeamID).ToListAsync(),
                Options = await _context.PaymentOptions.Where(o => o.TeamID == team.TeamID).ToListAsync(),
                Service = new Service()
            };

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);
            var personnel = await _context.Personnels.FirstOrDefaultAsync(p => p.ID == id);
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == Model.Service.ProductID);
            var option = await _context.PaymentOptions.FirstOrDefaultAsync(p => p.ID == Model.Service.PaymentOptionID);

            var total = product.Price + (product.Price * option.Ratio / 100);

            Model.Service.PersonnelID = personnel.ID;
            Model.Service.TeamID = team.TeamID;
            Model.Service.Total = total;
            Model.Service.CreatedAt = DateTime.Now;

            _context.Services.Add(Model.Service);

            int dayCounter = 0;
            decimal remainingAmount = Model.Service.Total;

            for (int i = 0; i < option.Times; i++)
            {
                Payment payment = new Payment()
                {
                    TeamID = team.TeamID,
                    ServiceID = Model.Service.ID,
                    Amount = Model.Service.Total / option.Times,
                    RemainingAmount = remainingAmount - (Model.Service.Total / option.Times),
                    PaymentOn = DateTime.Now.Date.AddDays(dayCounter),
                    CreatedAt = DateTime.Now
                };

                remainingAmount = remainingAmount - (Model.Service.Total / option.Times);
                dayCounter = dayCounter + 30;

                _context.Payments.Add(payment);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            Service service = await _context.Services
                                    .Include(s => s.Payments)
                                    .Include(s => s.PaymentOption)
                                    .Include(s => s.Product)
                                    .Include(s => s.Personnel)
                                    .ThenInclude(s => s.Firm)
                                    .FirstOrDefaultAsync(s => s.ID == id);

            if (service == null)
                return NotFound();

            return View(service);
        }
    }
}