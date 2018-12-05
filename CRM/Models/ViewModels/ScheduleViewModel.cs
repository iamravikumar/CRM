using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModels
{
    public class ScheduleViewModel
    {
        public IEnumerable<Programme> Programmes { get; set; }
        public Schedule Schedule { get; set; }
    }
}
