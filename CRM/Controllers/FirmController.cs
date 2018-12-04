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
    public class FirmController : Controller
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public FirmViewModel Model { get; set; }

        public FirmController(ApplicationDbContext context)
        {
            _context = context;

            Model = new FirmViewModel()
            {
                Sectors = _context.Sectors.Where(s => s.IsActive == true).ToList(),
                Firm = new Firm()
            };
        }

        public async Task<IActionResult> Index()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);
            var firms = await _context.Firms.Where(f => f.TeamID == team.TeamID).Include(f => f.Sector).ToListAsync();

            return View(firms);
        }

        public IActionResult Create()
        {
            return View(Model);
        }

        [HttpPost, ActionName(nameof(Create))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Store()
        {
            if (!ModelState.IsValid)
                return View(Model);

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);

            Model.Firm.TeamID = team.TeamID;
            Model.Firm.CreatedAt = DateTime.Now;

            try
            {
                _context.Firms.Add(Model.Firm);
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

            Model.Firm = await _context.Firms.FirstOrDefaultAsync(f => f.ID == id);

            if (Model.Firm == null)
                return NotFound();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Firm firm)
        {
            if (!ModelState.IsValid)
                return View(Model);

            firm = await _context.Firms.FirstOrDefaultAsync(f => f.ID == id);

            try
            {
                firm.SectorID = Model.Firm.SectorID;
                firm.Name = Model.Firm.Name;
                firm.Description = Model.Firm.Description;
                firm.Email = Model.Firm.Email;
                firm.Phone = Model.Firm.Phone;
                firm.Fax = Model.Firm.Fax;
                firm.Province = Model.Firm.Province;
                firm.City = Model.Firm.City;
                firm.Country = Model.Firm.Country;
                firm.Address = Model.Firm.Address;
                firm.Website = Model.Firm.Website;
                firm.Division = Model.Firm.Division;

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

            Model.Firm = await _context.Firms.FirstOrDefaultAsync(f => f.ID == id);

            if (Model.Firm == null)
                return NotFound();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Model.Firm = await _context.Firms.FirstOrDefaultAsync(f => f.ID == id);

            try
            {
                _context.Firms.Remove(Model.Firm);
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