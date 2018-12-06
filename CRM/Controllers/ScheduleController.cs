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
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public ScheduleViewModel Model { get; set; }

        public ScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Schedules()
        {
            return Json(_context.Schedules.Include(s => s.Personnel).Include(s => s.Programme).AsEnumerable().Select(s => new {
                id = s.ID,
                title = s.Programme.Name + " with " + s.Personnel.FullName(),
                start = s.StartedAt.ToString("yyyy-MM-dd") + "T" + s.StartedAt.ToLongTimeString(),
                end = s.FinishedAt.ToString("yyyy-MM-dd") + "T" + s.FinishedAt.ToLongTimeString(),
                color = s.Programme.Color,
                url = "Schedule/Edit/" + s.ID
            }));
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

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);

            Model = new ScheduleViewModel()
            {
                Programmes = _context.Programmes.Where(p => p.TeamID == team.TeamID).Where(p => p.IsActive).ToList(),
                Schedule = new Schedule()
            };
            
            return View(Model);
        }

        [HttpPost, ActionName(nameof(Create))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Store(int id)
        {
            Personnel personnel = await _context.Personnels.FindAsync(id);

            if (personnel == null)
                return NotFound();

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);

            Model.Schedule.PersonnelID = personnel.ID;
            Model.Schedule.TeamID = team.TeamID;
            Model.Schedule.CreatedAt = DateTime.Now;

            try
            {
                _context.Schedules.Add(Model.Schedule);
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
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);

            Model = new ScheduleViewModel()
            {
                Programmes = _context.Programmes.Where(p => p.TeamID == team.TeamID).Where(p => p.IsActive).ToList(),
                Schedule = new Schedule()
            };

            if (id == null)
                return NotFound();

            Model.Schedule = await _context.Schedules.FindAsync(id);

            if (Model.Schedule == null)
                return NotFound();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid)
                return View(Model);

            Schedule schedule = await _context.Schedules.FindAsync(id);

            try
            {
                schedule.StartedAt = Model.Schedule.StartedAt;
                schedule.FinishedAt = Model.Schedule.FinishedAt;
                schedule.ProgrammeID = Model.Schedule.ProgrammeID;

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