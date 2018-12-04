using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModels
{
    public class FirmViewModel
    {
        public Firm Firm { get; set; }
        public IEnumerable<Sector> Sectors { get; set; }
    }
}
