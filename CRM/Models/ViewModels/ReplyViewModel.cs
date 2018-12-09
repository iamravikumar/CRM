using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModels
{
    public class ReplyViewModel
    {
        public Message Message { get; set; }
        public Message Reply { get; set; }
    }
}
