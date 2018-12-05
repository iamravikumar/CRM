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
    public class PersonnelController : Controller
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public DirectoryViewModel Model { get; set; }

        public PersonnelController(ApplicationDbContext context)
        {
            _context = context;

            Model = new DirectoryViewModel()
            {
                Firms = _context.Firms.ToList(),
                Personnel = new Personnel()
            };
        }

        [Route("Directory")]
        public async Task<IActionResult> Index()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await _context.ApplicationUsers.FindAsync(claim.Value);
            var team = await _context.TeamMembers.FirstOrDefaultAsync(t => t.UserID == user.Id);
            var personnels = await _context.Personnels.Where(f => f.TeamID == team.TeamID).Include(f => f.Firm).ToListAsync();

            return View(personnels);
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

            Model.Personnel.TeamID = team.TeamID;
            Model.Personnel.CreatedAt = DateTime.Now;

            try
            {
                _context.Personnels.Add(Model.Personnel);
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

            Model.Personnel = await _context.Personnels.FirstOrDefaultAsync(f => f.ID == id);

            if (Model.Personnel == null)
                return NotFound();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Personnel personnel)
        {
            if (!ModelState.IsValid)
                return View(Model);

            personnel = await _context.Personnels.FirstOrDefaultAsync(f => f.ID == id);

            try
            {
                personnel.FirmID = Model.Personnel.FirmID;
                personnel.FirstName = Model.Personnel.FirstName;
                personnel.LastName = Model.Personnel.LastName;
                personnel.Birthday = Model.Personnel.Birthday;
                personnel.Email = Model.Personnel.Email;
                personnel.Phone = Model.Personnel.Phone;
                personnel.City = Model.Personnel.City;
                personnel.Province = Model.Personnel.Province;
                personnel.Country = Model.Personnel.Country;
                personnel.Address = Model.Personnel.Address;

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

            Model.Personnel = await _context.Personnels.FirstOrDefaultAsync(f => f.ID == id);

            if (Model.Personnel == null)
                return NotFound();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Model.Personnel = await _context.Personnels.FirstOrDefaultAsync(f => f.ID == id);

            try
            {
                _context.Personnels.Remove(Model.Personnel);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            Personnel personnel = await _context.Personnels.Include(p => p.Firm).FirstOrDefaultAsync(f => f.ID == id);

            if (personnel == null)
                return NotFound();

            return View(personnel);
        }
    }
}