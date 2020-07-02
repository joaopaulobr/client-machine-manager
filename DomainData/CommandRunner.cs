using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain
{
    public class CommandRunner
    {
        [JsonPropertyName("type")]
        [Newtonsoft.Json.JsonConverter(typeof(TerminalTypeConverter))]
        public TerminalType TerminalType { get; set; }

        [JsonPropertyName("text")]
        public string Command { get; set; }

        [JsonPropertyName("directory")]
        public string Directory { get; set; }

        [JsonPropertyName("output")]
        public string Output { get; set; }

        public void RunCommand()
        {
            Process process = new Process();

            process.StartInfo = new ProcessStartInfo($"{TerminalType}.exe")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            process.Start();

            if (!String.IsNullOrEmpty(Directory))
            {
                process.StandardInput.WriteLine($"cd \"{Directory}\"");
            }

            process.StandardInput.WriteLine(Command);
            process.StandardInput.Close();

            List<string> lines = new List<string>();

            while (!process.StandardOutput.EndOfStream)
            {
                lines.Add(process.StandardOutput.ReadLine().Trim());
            }

            this.Directory = lines.Last();

            int skip = 5;
            int afterOutput = 3;

            if (TerminalType == TerminalType.Powershell)
            {
                skip += 1;
                afterOutput--;
                this.Directory = this.Directory.Substring(3, this.Directory.Length - 4);
            }
            else
            {
                if (String.Equals(Command, "cd")) skip++;
                this.Directory = this.Directory.Substring(3, this.Directory.Length - 3);
            }

            lines.RemoveRange(0, skip);
            int start = lines.Count() - afterOutput;
            if (start < 0)
            {
                start = 0;
                afterOutput = 1;
            }
            lines.RemoveRange(start, afterOutput);

            lines.ForEach(l => this.Output += l+"\r\n");
            process.WaitForExit();
        }

        public string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }

    public class TerminalTypeConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var value = (string)reader.Value;

            return TerminalType.Powershell;
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            value = (TerminalType)value;

            writer.WriteValue("0");
        }
    }

    public enum TerminalType
    {
        Cmd,
        Powershell
    }
}