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
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Inbox(int? id)
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            Message currentMessage;

            if (id != null)
                currentMessage = await _context.Messages.Include(m => m.Receiver).Where(m => m.ReceiverID == claim.Value).Where(m => m.ParentID == null).FirstOrDefaultAsync(m => m.ID == id);
            else
                currentMessage = await _context.Messages.Include(m => m.Receiver).Where(m => m.ReceiverID == claim.Value).Where(m => m.ParentID == null).OrderByDescending(m => m.CreatedAt).Take(1).FirstOrDefaultAsync();

            if (currentMessage == null)
                return NotFound();

            if (currentMessage.IsViewed != true)
            {
                currentMessage.IsViewed = true;
                await _context.SaveChangesAsync();
            }

            InboxViewModel inboxModel = new InboxViewModel()
            {
                Inbox = await _context.Messages.Where(i => i.ReceiverID == claim.Value).Include(i => i.Writer).OrderByDescending(i => i.CreatedAt).ToListAsync(),
                Message = currentMessage
            };

            return View(inboxModel);
        }

        public async Task<IActionResult> Send(string id)
        {
            if (id == null)
                return NotFound();

            ApplicationUser user = await _context.ApplicationUsers.FindAsync(id);

            if (user == null)
                return NotFound();

            MessageViewModel messageModel = new MessageViewModel()
            {
                Receiver = user,
                Message = new Message()
            };

            return View(messageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(string id, MessageViewModel messageModel)
        {
            if (!ModelState.IsValid)
                return View(messageModel);

            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            messageModel.Message.WriterID = claim.Value;
            messageModel.Message.ReceiverID = id;
            messageModel.Message.IsViewed = false;
            messageModel.Message.CreatedAt = DateTime.Now;

            try
            {
                _context.Messages.Add(messageModel.Message);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e + ": An error occurred.");
            }

            return RedirectToAction(nameof(Inbox));
        }
    }
}