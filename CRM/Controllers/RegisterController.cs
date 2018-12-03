using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using CRM.Models.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RegisterController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registration)
        {
            if (!ModelState.IsValid)
                return View(registration);

            var user = new ApplicationUser()
            {
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                UserName = registration.Email,
                Email = registration.Email,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, registration.Password);

            if (!await _roleManager.RoleExistsAsync("Officer"))
                await _roleManager.CreateAsync(new IdentityRole("Officer"));

            if (!await _roleManager.RoleExistsAsync("Member"))
                await _roleManager.CreateAsync(new IdentityRole("Member"));

            if (!await _roleManager.RoleExistsAsync("PassiveMember"))
                await _roleManager.CreateAsync(new IdentityRole("PassiveMember"));

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors.Select(e => e.Description))
                {
                    ModelState.AddModelError("", error);
                }

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Login");
        }
    }
}