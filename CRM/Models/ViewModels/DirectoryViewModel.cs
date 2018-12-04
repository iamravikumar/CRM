using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModels
{
    public class DirectoryViewModel
    {
        public Personnel Personnel { get; set; }
        public IEnumerable<Firm> Firms { get; set; }
    }
}
