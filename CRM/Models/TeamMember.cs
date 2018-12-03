using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class TeamMember
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "User")]
        public string UserID { get; set; }

        [Display(Name = "Team")]
        public int TeamID { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("TeamID")]
        public virtual Team Team { get; set; }
    }
}
