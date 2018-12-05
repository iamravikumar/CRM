using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    public class NoteTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NoteTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);
            var sectors = await _context.NoteTypes.Where(s => s.TeamID == team.TeamID).ToListAsync();

            return View(sectors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteType type)
        {
            if (!ModelState.IsValid)
                return NotFound(type);

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);

            type.CreatedAt = DateTime.Now;
            type.TeamID = team.TeamID;

            try
            {
                _context.NoteTypes.Add(type);
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

            NoteType type = await _context.NoteTypes.FindAsync(id);

            if (type == null)
                return NotFound();

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteType type)
        {
            if (id != type.ID)
                return NotFound();

            var existingType = await _context.NoteTypes.FindAsync(id);

            try
            {
                existingType.Name = type.Name;
                existingType.IsActive = type.IsActive;

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

            NoteType type = await _context.NoteTypes.FindAsync(id);

            if (type == null)
                return NotFound();

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            NoteType type = await _context.NoteTypes.FindAsync(id);

            try
            {
                _context.NoteTypes.Remove(type);
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