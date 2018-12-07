using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.ViewModels
{
    public class InboxViewModel
    {
        public IEnumerable<Message> Inbox { get; set; }
        public Message Message { get; set; }
    }
}
