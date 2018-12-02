using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using CRM.Utilities;
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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.Include(t => t.Members).ToListAsync());
        }

        [Authorize(Roles = StaticEnvironments.User)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = StaticEnvironments.User)]
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
                    UserID = claim.Value
                };

                _context.TeamMembers.Add(member);

                await _userManager.RemoveFromRoleAsync(user, StaticEnvironments.User);
                await _userManager.AddToRoleAsync(user, StaticEnvironments.Officer);
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