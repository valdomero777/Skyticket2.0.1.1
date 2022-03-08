using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyticket
{
    public class TicketChoice
    {
        public string targetInput { get; set; }
        public TicketMethod printMethod { get; set; }

        public TicketChoice()
        {
            targetInput = "";
            printMethod = TicketMethod.None;
        }
    }

    public enum TicketMethod
    {
        None = 0,
        Paper = 1,
        Whatsapp = 2,
        NoPrint = 3,
        SMS = 4,
        Email = 5,
        Batch = 6,
    }
}