using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableMSSQLAdministration.Models
{
    public class TCPPortCheck
    {
        public bool IsOpen { get; set; }
        public string IPAddress { get; set; }
    }
}
