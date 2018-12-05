using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Note
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Personnel")]
        public int PersonnelID { get; set; }

        [Display(Name = "Note Type")]
        public int NoteTypeID { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("PersonnelID")]
        public virtual Personnel Personnel { get; set; }

        [ForeignKey("NoteTypeID")]
        public virtual NoteType NoteType { get; set; }
    }
}
