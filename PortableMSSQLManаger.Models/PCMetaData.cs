using System.Collections.Generic;

namespace PortableMSSQLAdministration.Models
{
    public class PCMetaData
    {
        public string CPU { get; set; }
        public int NumberCPU { get; set; }
        public long MemoryMB { get; set; }
        public List<DiskInformation> DiskInformation { get; set; }
        public string Name { get; set; }
        public string WinUserName { get; set; }
        public string ExternalIP { get; set; }
        public List<NetworkAdapterData> LanData { get; set; }
        public string OSName { get; set; }
        public string OSVersion { get; set; }
        public string OSArch { get; set; }
    }
 
}
