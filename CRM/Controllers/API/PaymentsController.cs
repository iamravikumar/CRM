using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // PUT: api/Schedules/5
        [HttpPut("{id}")]
        public IActionResult Put(int id)
        {
            var payment = _context.Payments.Find(id);

            try
            {
                if (payment.IsDone == true)
                    payment.IsDone = false;
                else
                    payment.IsDone = true;

                payment.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
            }

            return Ok();
        }
    }
}
