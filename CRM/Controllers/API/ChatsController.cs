using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Data;
using CRM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Chats
        [HttpGet]
        public IActionResult Get()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = _context.TeamMembers.FirstOrDefault(t => t.UserID == claim.Value);

            var chats = _context.TeamChats.Where(c => c.TeamID == team.TeamID).Include(c => c.User).Select(c => new {
                user = c.User.FullName(),
                body = c.Body,
            });

            return Ok(chats);
        }

        // GET: api/Chats/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Chats
        [HttpPost]
        public IActionResult Post([FromBody] TeamChat chat)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var team = _context.TeamMembers.FirstOrDefault(t => t.UserID == claim.Value);

            chat.UserID = claim.Value;
            chat.TeamID = team.TeamID;
            chat.CreatedAt = DateTime.Now;

            try
            {
                _context.TeamChats.Add(chat);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }

            return Ok();
        }

        // PUT: api/Chats/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
