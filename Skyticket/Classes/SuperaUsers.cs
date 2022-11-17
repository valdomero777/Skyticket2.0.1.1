using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyticket.Classes
{
    public class SuperaUsers
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string sucursal { get; set; }
        public Int64 telefono { get; set; }

    }

    public class UsersRes
    {
        public SuperaUsers users { get; set; }
    }
}
