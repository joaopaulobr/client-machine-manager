using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerAPI
{
    public abstract class WebSocketHandler
    {
        protected ConnectionManager WebSocketConnectionManager { get; set; }

        public WebSocketHandler(ConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual async Task<TaskCompletionSource<object>> OnConnected(WebSocket socket)
        {
            TaskCompletionSource<object> taskCompletionSource;
            WebSocketConnectionManager.AddSocket(socket, out taskCompletionSource);

            return taskCompletionSource;
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
        }

        public async Task<CommandRunner> SendMessageAsync(WebSocket socket, string message)
        {
            CommandRunner cr = null;

            if (socket == null || socket.State != WebSocketState.Open)
            {
                cr = new CommandRunner
                {
                    Directory = "CONEXÃO FECHADA COM A MÁQUINA CLIENTE"
                };
                return cr;
            }

            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
            var buffer = new byte[1024 * 4];

            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            cr = System.Text.Json.JsonSerializer.Deserialize<CommandRunner>(Encoding.UTF8.GetString(buffer, 0, result.Count));

            return cr;
        }

        public async Task<CommandRunner> SendMessageAsync(string socketId, string message)
        {
            return await SendMessageAsync(WebSocketConnectionManager.GetSocketById(socketId), message);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in WebSocketConnectionManager.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, message);
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
