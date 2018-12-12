using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Payment
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Team")]
        public int? TeamID { get; set; }

        [Display(Name = "Service")]
        public int ServiceID { get; set; }

        public decimal Amount { get; set; }

        [Display(Name = "Remaining Amount")]
        public decimal RemainingAmount { get; set; }

        [DataType(DataType.Date)]
        public DateTime PaymentOn { get; set; }

        public bool IsDone { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("TeamID")]
        public virtual Team Team { get; set; }

        [ForeignKey("ServiceID")]
        public virtual Service Service { get; set; }
    }
}
