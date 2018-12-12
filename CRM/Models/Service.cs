using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Service
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Team")]
        public int? TeamID { get; set; }

        [Display(Name = "Product")]
        public int? ProductID { get; set; }

        [Display(Name = "Personnel")]
        public int? PersonnelID { get; set; }

        [Display(Name = "Payment Option")]
        public int PaymentOptionID { get; set; }

        public decimal Total { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("TeamID")]
        public virtual Team Team { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [ForeignKey("PersonnelID")]
        public virtual Personnel Personnel { get; set; }

        [ForeignKey("PaymentOptionID")]
        public virtual PaymentOption PaymentOption { get; set; }

        public virtual IEnumerable<Payment> Payments { get; set; }
    }
}
