using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModels
{
    public class ScheduleDashboardViewModel
    {
        public IEnumerable<Schedule> ScheduleHistories { get; set; }
        public IEnumerable<Schedule> UpcomingSchedules { get; set; }
        public TeamMember Member { get; set; }
    }
}
