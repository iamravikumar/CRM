using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModels
{
    public class MessageViewModel
    {
        public ApplicationUser Receiver { get; set; }
        public Message Message { get; set; }
    }
}
