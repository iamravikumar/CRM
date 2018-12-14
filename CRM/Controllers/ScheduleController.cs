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

        public async Task<IActionResult> Index()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);

            ScheduleDashboardViewModel scheduleModel = new ScheduleDashboardViewModel()
            {
                Member = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value),
                UpcomingSchedules = await _context.Schedules
                                            .Where(s => s.TeamID == team.TeamID)
                                            .Where(s => s.IsDone == false)
                                            .Include(s => s.User)
                                            .Include(s => s.Personnel)
                                            .Include(s => s.Programme)
                                            .OrderBy(s => s.StartedAt)
                                            .ToListAsync(),
                ScheduleHistories = await _context.Schedules
                                            .Where(s => s.TeamID == team.TeamID)
                                            .Where(s => s.IsDone == true)
                                            .Where(s => s.UpdatedAt.Date == DateTime.Now.Date)
                                            .Include(s => s.User)
                                            .Include(s => s.Personnel)
                                            .Include(s => s.Programme)
                                            .OrderBy(s => s.StartedAt)
                                            .ToListAsync()
            };

            return View(scheduleModel);
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
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);

            Model.Schedule.UserID = claim.Value;
            Model.Schedule.PersonnelID = personnel.ID;
            Model.Schedule.TeamID = team.TeamID;
            Model.Schedule.CreatedAt = DateTime.Now;
            Model.Schedule.UpdatedAt = DateTime.Now;

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
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);

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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Schedule sector = await _context.Schedules.Include(s => s.Personnel).Include(s => s.User).FirstOrDefaultAsync(s=> s.ID == id);

            if (sector == null)
                return NotFound();

            return View(sector);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Schedule schedule = await _context.Schedules.FindAsync(id);

            try
            {
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);

            var schedules = await _context.Schedules
                        .Where(s => s.TeamID == team.TeamID)
                        .Include(s => s.User)
                        .Include(s => s.Personnel)
                        .Include(s => s.Programme)
                        .OrderByDescending(s => s.StartedAt)
                        .ToListAsync();

            return View(schedules);
        }

        public async Task<IActionResult> Upcoming()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);

            var schedules = await _context.Schedules
                        .Where(s => s.TeamID == team.TeamID)
                        .Where(s => s.IsDone == false)
                        .Include(s => s.User)
                        .Include(s => s.Personnel)
                        .Include(s => s.Programme)
                        .OrderByDescending(s => s.StartedAt)
                        .ToListAsync();

            return View(schedules);
        }

        public async Task<IActionResult> History()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == claim.Value);

            var schedules = await _context.Schedules
                        .Where(s => s.TeamID == team.TeamID)
                        .Where(s => s.IsDone == true)
                        .Include(s => s.User)
                        .Include(s => s.Personnel)
                        .Include(s => s.Programme)
                        .OrderByDescending(s => s.StartedAt)
                        .ToListAsync();

            return View(schedules);
        }
    }
}