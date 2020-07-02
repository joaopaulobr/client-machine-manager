using Domain;
using Newtonsoft.Json;
using NUnit.Framework;
using ClientMachineManager.Domain;

namespace DomainTest
{
    public class CommandRunnerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("cd")]
        [TestCase("cd ..")]
        public void RunCmdCommandWithouOutput(string value)
        {
            CommandRunner cr = new CommandRunner()
            {
                TerminalType = TerminalType.Cmd,
                Command = value
            };

            cr.RunCommand();

            Assert.IsNull(cr.Output);
            Assert.IsNotNull(cr.Directory);
        }

        [Test]
        [TestCase("dir")]
        public void RunCmdCommandWithOutput(string value)
        {
            CommandRunner cr = new CommandRunner()
            {
                TerminalType = TerminalType.Cmd,
                Command = value
            };

            cr.RunCommand();

            Assert.IsNotNull(cr.Output);
            Assert.IsNotNull(cr.Directory);
        }

        [Test]
        [TestCase("cd ..")]
        [TestCase("cd")]
        public void RunCmdCommandOnPowershellWithoutOutput(string value)
        {
            CommandRunner cr = new CommandRunner()
            {
                TerminalType = TerminalType.Powershell,
                Command = value
            };

            cr.RunCommand();

            Assert.IsNull(cr.Output);
            Assert.IsNotNull(cr.Directory);
        }

        [Test]
        [TestCase("dir")]
        public void RunCmdCommandOnPowershellWithOutput(string value)
        {
            CommandRunner cr = new CommandRunner()
            {
                TerminalType = TerminalType.Powershell,
                Command = value
            };

            cr.RunCommand();

            Assert.IsNotNull(cr.Output);
            Assert.IsNotNull(cr.Directory);
        }

        [Test]
        [TestCase("ls")]
        public void RunPowershellCommand(string value)
        {
            CommandRunner cr = new CommandRunner()
            {
                TerminalType = TerminalType.Powershell,
                Command = value
            };

            cr.RunCommand();

            Assert.IsNotNull(cr.Output);
            Assert.IsNotNull(cr.Directory);
        }

        [Test]
        [TestCase(TerminalType.Powershell)]
        [TestCase(TerminalType.Cmd)]
        public void JsonSerialization(TerminalType value)
        {
            CommandRunner cr = new CommandRunner()
            {
                TerminalType = value,
                Command = "dir"
            };

            string json = cr.ToJson();

            CommandRunner deserializedJson = JsonConvert.DeserializeObject<CommandRunner>(json);
            Assert.AreEqual(cr.ToString(), deserializedJson.ToString());
        }
    }
}