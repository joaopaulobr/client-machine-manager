using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ClientMachineManager.Domain;

namespace ClientService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ClientWebSocket clientSocket = new ClientWebSocket();
            await clientSocket.ConnectAsync(new Uri("ws://localhost:49256/ws"), CancellationToken.None);

            var bytes = Encoding.UTF8.GetBytes(new MachineInfo().ToJson());
            await clientSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);

            Task receiving = Receiving(clientSocket);
        }

        private static async Task Receiving(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];

            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                CommandRunner cr = System.Text.Json.JsonSerializer.Deserialize<CommandRunner>(Encoding.UTF8.GetString(buffer, 0, result.Count));

                if (result.MessageType == WebSocketMessageType.Text) {
                    cr.RunCommand();

                    var bytes = Encoding.UTF8.GetBytes(cr.ToJson());
                    await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
            }
        }
    }
}