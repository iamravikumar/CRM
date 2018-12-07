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
    public class SectorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SectorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);
            var sectors = await _context.Sectors.Where(s => s.TeamID == team.TeamID).ToListAsync();

            return View(sectors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sector sector)
        {
            if (!ModelState.IsValid)
                return View(sector);

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);

            sector.CreatedAt = DateTime.Now;
            sector.TeamID = team.TeamID;

            try
            {
                _context.Sectors.Add(sector);
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

            Sector sector = await _context.Sectors.FindAsync(id);

            if (sector == null)
                return NotFound();

            return View(sector);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sector sector)
        {
            if (id != sector.ID)
                return NotFound();

            var existingSector = await _context.Sectors.FindAsync(id);

            try
            {
                existingSector.Name = sector.Name;
                existingSector.IsActive = sector.IsActive;

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

            Sector sector = await _context.Sectors.FindAsync(id);

            if (sector == null)
                return NotFound();

            return View(sector);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Sector sector = await _context.Sectors.FindAsync(id);

            try
            {
                _context.Sectors.Remove(sector);
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