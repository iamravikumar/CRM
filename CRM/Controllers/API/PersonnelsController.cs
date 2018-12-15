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
    public class PersonnelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PersonnelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Get()
        {
            var personnels = _context.Personnels.Select(p => new {
                name = p.FullName()
            }).ToList();

            return Ok(personnels);
        }
    }
}
