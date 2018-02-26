using System.Collections.Generic;

namespace PortableMSSQLAdministration.Models
{
    public class NetworkAdapterData
    {
        public string AdapterName { get; set; }
        public string AdapterDescription { get; set; }
        public string AdapterMac { get; set; }
        public string AdapterIP { get; set; }
        public string AdapterGatewayIP { get; set; }
        public string AdapterIPMode { get; set; }
        public List<string> AdapterDNSIps { get; set; }
    }
}
