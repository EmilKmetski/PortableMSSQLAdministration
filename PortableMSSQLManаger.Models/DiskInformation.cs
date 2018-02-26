namespace PortableMSSQLAdministration.Models
{
    public class DiskInformation
    {
        public string DriveLetter { get; set; }
        public long FreeSpaceMB { get; set; }
        public long TotalSpaceMB { get; set; }
    }
}
