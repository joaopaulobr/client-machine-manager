using Newtonsoft.Json;
using NUnit.Framework;
using ClientMachineManager.Domain;

namespace DomainTest
{
    public class MachineInfoTest
    {
        MachineInfo machineInfo = new MachineInfo();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NameNotNull()
        {
            Assert.IsNotNull(machineInfo.MachineName);
        }

        [Test]
        public void LocalIPNotNull()
        {
            Assert.IsNotNull(machineInfo.LocalIP);
        }

        [Test]
        public void AntivirusNotNull()
        {
            Assert.IsNotNull(machineInfo.Antivirus);
        }

        [Test]
        public void IsFirewallActiveNotNull()
        {
            Assert.IsNotNull(machineInfo.IsFirewallActive);
        }

        [Test]
        public void DotNetFrameworkNotNull()
        {
            Assert.IsNotNull(machineInfo.DotNetFrameworkVersion);
        }

        [Test]
        public void WindowsVersionNotNull()
        {
            Assert.IsNotNull(machineInfo.WindowsVersion);
        }

        [Test]
        public void TotalHardDriveSizeNotNull()
        {
            Assert.IsNotNull(machineInfo.TotalHardDriveSize);
        }

        [Test]
        public void AvailableHardDriveSizeNotNull()
        {
            Assert.IsNotNull(machineInfo.AvailableHardDriveSize);
        }

        [Test]
        public void JsonSerialization()
        {
            string json = machineInfo.ToJson();

            MachineInfo deserializedJson = JsonConvert.DeserializeObject<MachineInfo>(json);
            Assert.AreEqual(machineInfo.ToString(), deserializedJson.ToString());
        }
    }
}