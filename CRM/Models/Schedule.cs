using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Schedule
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Personnel")]
        public int PersonnelID { get; set; }

        [Display(Name = "Team")]
        public int? TeamID { get; set; }

        [Display(Name = "Programme")]
        public int? ProgrammeID { get; set; }

        [Display(Name = "Started At")]
        public DateTime StartedAt { get; set; }

        [Display(Name = "Finished At")]
        public DateTime FinishedAt { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("PersonnelID")]
        public virtual Personnel Personnel { get; set; }

        [ForeignKey("TeamID")]
        public virtual Team Team { get; set; }

        [ForeignKey("ProgrammeID")]
        public virtual Programme Programme { get; set; }
    }
}
