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
    public class ProgrammeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgrammeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);
            var programmes = await _context.Programmes.Where(s => s.TeamID == team.TeamID).ToListAsync();

            return View(programmes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Programme programme)
        {
            if (!ModelState.IsValid)
                return NotFound(programme);

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);

            programme.CreatedAt = DateTime.Now;
            programme.TeamID = team.TeamID;

            try
            {
                _context.Programmes.Add(programme);
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

            Programme programme = await _context.Programmes.FindAsync(id);

            if (programme == null)
                return NotFound();

            return View(programme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Programme programme)
        {
            if (id != programme.ID)
                return NotFound();

            var existingProgramme = await _context.Programmes.FindAsync(id);

            try
            {
                existingProgramme.Name = programme.Name;
                existingProgramme.IsActive = programme.IsActive;

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

            Programme programme = await _context.Programmes.FindAsync(id);

            if (programme == null)
                return NotFound();

            return View(programme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Programme programme = await _context.Programmes.FindAsync(id);

            try
            {
                _context.Programmes.Remove(programme);
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