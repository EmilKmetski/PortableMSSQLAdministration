using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PortableMSSQLAdministration.DAL;
using PortableMSSQLAdministration.Models;

namespace PortableMSSQLAdministration.BL
{
    public static class WindowsMetaInfo
    {
        public static PCMetaData GetPCMetaData()
        {
            return MetaDataGenerator.GetPCMetaData();
        }
    }

    public static class OpenPortChecker
    {
        private static List<TCPPortCheck> CheckPorts(List<NetworkAdapterData> IPs, int PortNumber)
        {
            List<TCPPortCheck> IPchecks = new List<TCPPortCheck>();
            foreach (var adapterData in MetaDataGenerator.GetPCMetaData().LanData)
            {
                TCPPortCheck currentPortCheck = new TCPPortCheck();
                try
                {
                    var clientWAN = new TcpClient();
                    var resultWAN = clientWAN.BeginConnect(adapterData.AdapterIP, PortNumber, null, null);

                    resultWAN.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                    if (!clientWAN.Connected)
                    {
                        currentPortCheck.IPAddress = adapterData.AdapterIP;
                        currentPortCheck.IsOpen = false;
                        IPchecks.Add(currentPortCheck);
                    }
                    else
                    {
                        currentPortCheck.IPAddress = adapterData.AdapterIP;
                        currentPortCheck.IsOpen = true;
                        clientWAN.EndConnect(resultWAN);
                        IPchecks.Add(currentPortCheck);
                    }

                }
                catch { }
            }
            return IPchecks;
        }
    }

    public static class WindowsCommands
    {
        //Opens the added ports in windows fire wall for multiple ports comma separated
        //Need to check is it the old syntax or the new one
        private static void OpenPortsInFireWall(string PortsName, string Ports)
        {
            ExecuteCommand("netsh advfirewall firewall add rule name=" + PortsName + " dir=in action=allow protocol=TCP localport=" + Ports);
        }

        //opens windows firewall advanced settings
        private static void FireWall()
        {
            ExecuteCommand("wf.msc");
        }
        //opens windows control panel
        private static void ControlPanel()
        {
            ExecuteCommand("control");
        }

        //opens windows update works on all windows exept windows 10
        private static void WindowsUpdate()
        {
            ExecuteCommand("wuapp");
        }

        //opens windows Computer Management
        private static void ComputerManagement()
        {
            ExecuteCommand("compmgmt.msc");
        }

        //opens windows printes
        private static void Printers()
        {
            ExecuteCommand("control printers");
        }

        //opens windwos Administrative tools 
        private static void AdminTools()
        {
            ExecuteCommand("control admintools");
        }

        //opens windows programs and futures
        private static void ProgramsAndFutures()
        {
            ExecuteCommand("appwiz.cpl");
        }

        private static void SQLServerManager()
        {
            //implement based on the sql version switch to choose the correct EXE file
            ExecuteCommand("appwiz.cpl");
        }

        private static void ExecuteCommand(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", "/C " + command);
            // startInfo.FileName = @"C:\Windows\system32\cmd.exe";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process exeProcess = Process.Start(startInfo);
        }
    }
}
