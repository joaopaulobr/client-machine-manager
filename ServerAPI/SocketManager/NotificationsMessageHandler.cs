using Domain;
using Newtonsoft.Json;
using ServerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using ClientMachineManager.Domain;

namespace ServerAPI
{
    public class NotificationsMessageHandler : WebSocketHandler
    {
        public NotificationsMessageHandler(ConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            MachineInfo machineInfo = null;
            CommandRunner command = null;

            machineInfo = JsonConvert.DeserializeObject<MachineInfo>(Encoding.UTF8.GetString(buffer, 0, result.Count));
            MachinesRepository.Add(machineInfo);
            if(WebSocketConnectionManager.GetSocketById(machineInfo.LocalIP) == null)
            {
                WebSocketConnectionManager.UpdateSocketKey(socket, machineInfo.LocalIP);
            }
        }
    }
}
