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
    public class ServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Get(int productID, int optionID)
        {
            var product = _context.Products.Find(productID);
            var option = _context.PaymentOptions.Find(optionID);

            decimal total = product.Price + (product.Price * option.Ratio / 100);

            return Ok(total);
        }
    }
}
