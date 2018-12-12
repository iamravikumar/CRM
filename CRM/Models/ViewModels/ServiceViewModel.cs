using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModels
{
    public class ServiceViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<PaymentOption> Options { get; set; }
        public Service Service { get; set; }
    }
}
