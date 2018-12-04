using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Team")]
        public int TeamID { get; set; }

        [Display(Name = "Firm")]
        public int? FirmID { get; set; }

        [Display(Name = "Fisrt Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public string Birthday { get; set; }

        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("TeamID")]
        public virtual Team Team { get; set; }

        [ForeignKey("FirmID")]
        public virtual Firm Firm { get; set; }
    }
}
