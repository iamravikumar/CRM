using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Officer, Member")]
    public class SchedulesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Schedules
        [HttpGet]
        public IActionResult Get()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = _context.ApplicationUsers.Find(claim.Value);
            var team = _context.TeamMembers.FirstOrDefault(t => t.UserID == user.Id);

            var schedules = _context.Schedules.Where(s => s.TeamID == team.TeamID).Where(s => s.IsDone == false).Include(s => s.Personnel).Include(s => s.User).Include(s => s.Programme).AsEnumerable().Select(s => new
            {
                title = s.Programme.Name + " with " + s.Personnel.FullName() + " and " + s.User.FullName(),
                start = s.StartedAt.ToString("yyyy-MM-dd") + "T" + s.StartedAt.ToLongTimeString(),
                end = s.FinishedAt.ToString("yyyy-MM-dd") + "T" + s.FinishedAt.ToLongTimeString(),
                color = s.Programme.Color,
                url = "Schedule/Edit/" + s.ID
            });

            return Ok(schedules);
        }

        // PUT: api/Schedules/5
        [HttpPut("{id}")]
        public IActionResult Put(int id)
        {
            var schedule = _context.Schedules.Find(id);

            schedule.IsDone = true;
            _context.SaveChanges();

            return Ok();
        }
    }
}
