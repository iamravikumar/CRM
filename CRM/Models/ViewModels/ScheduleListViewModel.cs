using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModels
{
    public class ScheduleListViewModel
    {
        public IEnumerable<Schedule> CompletedSchedules { get; set; }
        public IEnumerable<Schedule> UnCompletedSchedules { get; set; }
        public TeamMember Member { get; set; }
    }
}
