using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Message
    {
        public int ID { get; set; }

        public string WriterID { get; set; }

        public string ReceiverID { get; set; }

        public int? ParentID { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Body { get; set; }

        public bool IsViewed { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("WriterID")]
        public virtual ApplicationUser Writer { get; set; }

        [ForeignKey("ReceiverID")]
        public virtual ApplicationUser Receiver { get; set; }

        [ForeignKey("ParentID")]
        public virtual IEnumerable<Message> Messages { get; set; }
    }
}
