using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyticket.Classes
{
   public class Ticket
    {
        public string _id { get; set; }
        public int id_terminal { get; set; }
        public int id_client { get; set; }
        public string ticketimagepath { get; set; }
        public string printmethod { get; set; }
        public string email { get; set; }
        public string mobilephone { get; set; }
        public bool sent { get; set; }
        public string datesent { get; set; }
        public string details { get; set; }
       
    }

    public class TicketRes
    {
        public Ticket ticket { get; set; }
    }
}
