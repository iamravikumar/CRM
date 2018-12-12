using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    [Authorize(Roles = "Officer")]
    public class PaymentOptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentOptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);
            var options = await _context.PaymentOptions.Where(s => s.TeamID == team.TeamID).ToListAsync();

            return View(options);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentOption option)
        {
            if (!ModelState.IsValid)
                return View(option);

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);

            option.CreatedAt = DateTime.Now;
            option.TeamID = team.TeamID;

            try
            {
                _context.PaymentOptions.Add(option);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            PaymentOption option = await _context.PaymentOptions.FindAsync(id);

            if (option == null)
                return NotFound();

            return View(option);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PaymentOption option)
        {
            if (id != option.ID)
                return NotFound();

            var existingOption = await _context.PaymentOptions.FindAsync(id);

            try
            {
                existingOption.Name = option.Name;
                existingOption.Ratio = option.Ratio;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            PaymentOption option = await _context.PaymentOptions.FindAsync(id);

            if (option == null)
                return NotFound();

            return View(option);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            PaymentOption option = await _context.PaymentOptions.FindAsync(id);

            try
            {
                _context.PaymentOptions.Remove(option);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}