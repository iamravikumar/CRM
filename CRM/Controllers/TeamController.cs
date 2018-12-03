using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TeamController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "User, PassiveMember")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.ToListAsync());
        }

        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team)
        {
            if (!ModelState.IsValid)
                return View(team);

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _context.ApplicationUsers.FindAsync(claim.Value);

            try
            {
                team.CreatedAt = DateTime.Now;
                _context.Teams.Add(team);

                TeamMember member = new TeamMember()
                {
                    TeamID = team.ID,
                    UserID = claim.Value,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.TeamMembers.Add(member);

                await _userManager.RemoveFromRoleAsync(user, "User");
                await _userManager.AddToRoleAsync(user, "Officer");

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Appeal(int? id)
        {
            if (id == null)
                return NotFound();

            Team team = await _context.Teams.FindAsync(id);

            if (team == null)
                return NotFound();

            return View(team);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Appeal(int id)
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _context.ApplicationUsers.FindAsync(claim.Value);

            TeamMember member = new TeamMember()
            {
                UserID = claim.Value,
                TeamID = id,
                IsActive = false,
                CreatedAt = DateTime.Now
            };

            try
            {
                await _userManager.RemoveFromRoleAsync(user, "User");
                await _userManager.AddToRoleAsync(user, "PassiveMember");

                _context.TeamMembers.Add(member);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Officer, Member")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> Applicants()
        {
            return View(await _context.TeamMembers.Where(t => t.IsActive == false).Include(t => t.User).ToListAsync());
        }

        [Authorize(Roles = "Officer")]
        [Route("Team/Applicant/{applicant}/Confirm/{id}")]
        public async Task<IActionResult> Confirm(string applicant, int? id)
        {
            if (id == null && applicant == null)
                return NotFound();

            var applicatan = await _context.TeamMembers.Include(t => t.User).FirstOrDefaultAsync(t => t.ID == id);

            if (applicatan == null)
                return NotFound();

            return View(applicatan);
        }

        [HttpPost]
        [Route("Team/Applicant/{applicant}/Confirm/{id}")]
        [Authorize(Roles = "Officer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(string applicant, int id, [Bind("ID,IsActive")] TeamMember member)
        {
            if (id != member.ID)
                return NotFound();

            var user = await _context.ApplicationUsers.FindAsync(applicant);

            try
            {
                member.IsActive = true;

                _context.TeamMembers.Update(member);

                _context.Entry(member).Property("CreatedAt").IsModified = false;
                _context.Entry(member).Property("UserID").IsModified = false;
                _context.Entry(member).Property("TeamID").IsModified = false;

                await _userManager.RemoveFromRoleAsync(user, "PassiveMember");
                await _userManager.AddToRoleAsync(user, "Member");

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize(Roles = "Officer")]
        [Route("Team/Applicant/{applicant}/Reject/{id}")]
        public async Task<IActionResult> Reject(string applicant, int? id)
        {
            if (id == null && applicant == null)
                return NotFound();

            var applicatan = await _context.TeamMembers.Include(t => t.User).FirstOrDefaultAsync(t => t.ID == id);

            if (applicatan == null)
                return NotFound();

            return View(applicatan);
        }

        [HttpPost]
        [Authorize(Roles = "Officer")]
        [Route("Team/Applicant/{applicant}/Reject/{id}")]
        public async Task<IActionResult> Reject(string applicant, int id)
        {
            var user = await _context.ApplicationUsers.FindAsync(applicant);
            TeamMember member = await _context.TeamMembers.FindAsync(id);

            try
            {
                await _userManager.RemoveFromRoleAsync(user, "PassiveMember");
                await _userManager.AddToRoleAsync(user, "User");

                _context.TeamMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Applicants));
        }

        [Authorize(Roles = "Officer, Member")]
        public async Task<IActionResult> Members()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);
            var members = await _context.TeamMembers.Include(t => t.User).Where(t => t.TeamID == team.TeamID).Where(t => t.IsActive == true).ToListAsync();

            return View(members);
        }
    }
}