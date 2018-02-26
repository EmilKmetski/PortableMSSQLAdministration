using PortableMSSQLAdministration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace PortableMSSQLAdministration.DAL
{
    public static class MetaDataGenerator
    {            
        public static PCMetaData GetPCMetaData()
        {
            PCMetaData currentData = new PCMetaData();
            currentData.CPU = GetHardwareInformation("Win32_Processor", "Name", false);
            currentData.NumberCPU = int.Parse(GetHardwareInformation("Win32_ComputerSystem", "NumberOfProcessors", false));
            currentData.MemoryMB = (Convert.ToInt64(GetHardwareInformation("Win32_PhysicalMemory", "Capacity", true)) / 1024L) / 1024L;
            currentData.DiskInformation = GetDisksInformation();
            currentData.Name = Environment.MachineName;
            currentData.WinUserName = Environment.UserName;
            currentData.OSName = GetHardwareInformation("Win32_OperatingSystem", "Caption", false);
            currentData.OSVersion = GetHardwareInformation("Win32_OperatingSystem", "Version", false);
            currentData.OSArch = GetHardwareInformation("Win32_OperatingSystem", "OSArchitecture", false);

            string externalIP = "";
            try
            {
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();
            }
            catch
            {
            }

            currentData.ExternalIP = externalIP;
            List<NetworkAdapterData> subcriberAdapters = new List<NetworkAdapterData>();
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if ((item.OperationalStatus == OperationalStatus.Up) && (item.Name.Contains("Loop") == false))
                {
                    try
                    {
                        IPInterfaceProperties adapterProperties = item.GetIPProperties();
                        IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();
                        NetworkAdapterData currentAdapter = new NetworkAdapterData();
                        currentAdapter.AdapterName = item.Name;
                        currentAdapter.AdapterDescription = item.Description;
                        currentAdapter.AdapterMac = item.GetPhysicalAddress().ToString();
                        currentAdapter.AdapterIPMode = (p.IsDhcpEnabled == true ? "DHCP" : "Static").ToString();

                        foreach (UnicastIPAddressInformation ip in adapterProperties.UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                currentAdapter.AdapterIP = ip.Address.ToString();
                            }
                        }
                        currentAdapter.AdapterGatewayIP = (adapterProperties.GatewayAddresses.Count > 0 ? adapterProperties.GatewayAddresses.First().Address.ToString() : "NA").ToString();
                        List<string> dnsIPs = new List<string>();
                        IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                        if (dnsServers != null)
                        {
                            foreach (IPAddress dns in dnsServers)
                            {
                                if (dns.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    dnsIPs.Add(dns.ToString());
                                }
                            }
                        }
                        currentAdapter.AdapterDNSIps = dnsIPs;
                        subcriberAdapters.Add(currentAdapter);
                    }
                    catch
                    {
                    }
                }
            }
            currentData.LanData = subcriberAdapters;
            return currentData;
        }
        private static List<DiskInformation> GetDisksInformation()
        {
            List<DiskInformation> currentDiskInfo = new List<DiskInformation>();

            ManagementObjectSearcher ms = new ManagementObjectSearcher("root\\CIMV2", "Select * FROM Win32_LogicalDisk WHERE DriveType = 3");
            foreach (ManagementObject mj in ms.Get())
            {
                DiskInformation diskInfo = new DiskInformation();
                diskInfo.DriveLetter = mj["Caption"].ToString();
                diskInfo.FreeSpaceMB = long.Parse(mj["FreeSpace"].ToString()) / 1024L / 1024L;
                diskInfo.TotalSpaceMB = long.Parse(mj["Size"].ToString()) / 1024L / 1024L;
                currentDiskInfo.Add(diskInfo);
            }
            return currentDiskInfo;
        }

        private static string GetHardwareInformation(string hwClass, string syntax, bool sumData)
        {
            string myResult = "";
            long result = 0;
            ManagementObjectSearcher ms = new ManagementObjectSearcher("root\\CIMV2", "Select * FROM " + hwClass);
            foreach (ManagementObject mj in ms.Get())
            {
                if (sumData)
                {
                    result += long.Parse(Convert.ToString(mj[syntax]));
                }
                else
                {
                    myResult = Convert.ToString(mj[syntax]);
                }
            }
            if (sumData)
            {
                myResult = result.ToString();
            }
            return myResult;
        }
    }
}
