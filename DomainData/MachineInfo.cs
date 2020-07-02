using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClientMachineManager.Domain
{
    public class MachineInfo
    {
        [JsonPropertyName("name")]
        public string MachineName { get; set; }

        [JsonPropertyName("ip")]
        public string LocalIP { get; set; }

        [JsonPropertyName("antivirus")]
        public string Antivirus { get; set; }

        [JsonPropertyName("is_firewall_active")]
        public bool? IsFirewallActive { get; set; }

        [JsonPropertyName("windows_version")]
        public string WindowsVersion { get; set; }

        [JsonPropertyName("dotnet_version")]
        public string DotNetFrameworkVersion { get; set; }

        [JsonPropertyName("total_hd_size")]
        public long TotalHardDriveSize { get; set; }

        [JsonPropertyName("free_hd_size")]
        public long AvailableHardDriveSize { get; set; }

        public MachineInfo()
        {
            Reevaluate();
        }

        public void Reevaluate()
        {
            MachineName = GetMachineName();
            LocalIP = GetLocalIPAddress();
            Antivirus = GetAntivirusName();
            IsFirewallActive = GetIsFirewallActive();
            WindowsVersion = GetOSVersion();
            DotNetFrameworkVersion = GetDotNetFrameworkVersion();

            long[] hardDriveSize = GetHardDriveSpace();
            TotalHardDriveSize = hardDriveSize[0];
            AvailableHardDriveSize = hardDriveSize[1];
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        #region Private Methods
        private string GetMachineName()
        {
            return Environment.MachineName;
        }

        private string GetOSVersion()
        {
            return Environment.OSVersion.VersionString;
        }

        private bool? GetIsFirewallActive()
        {
            return null;
        }

        private string GetDotNetFrameworkVersion()
        {
            return "Não verificado";
        }

        private string GetAntivirusName()
        {
            ManagementObjectSearcher wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
            ManagementObjectCollection data = wmiData.Get();

            string name = null;
            foreach (ManagementObject virusChecker in data)
            {
                name = virusChecker["displayName"].ToString();
                break;
            }
            return name;
        }

        private string GetLocalIPAddress()
        {
            string hostName = Dns.GetHostName();
            string localIP = Dns.GetHostEntry(hostName).AddressList.OrderBy(a => a.AddressFamily.ToString()).FirstOrDefault().ToString();
            return localIP;
        }

        private long[] GetHardDriveSpace()
        {
            long[] sizes = new long[2];
            long totalSize = 0;
            long totalFreeSize = 0;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    totalSize += drive.TotalSize;
                    totalFreeSize += drive.TotalFreeSpace;
                }
            }
            sizes[0] = totalSize;
            sizes[1] = totalFreeSize;
            return sizes;
        }
        #endregion
    }
}