using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace ServerAPI
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        private ConcurrentDictionary<string, TaskCompletionSource<object>> _socketsTaskCompletions = new ConcurrentDictionary<string, TaskCompletionSource<object>>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }
        public void AddSocket(WebSocket socket, out TaskCompletionSource<object> taskCompletionSource)
        {
            taskCompletionSource = new TaskCompletionSource<object>();
            string id = CreateConnectionId();

            _sockets.TryAdd(id, socket);
            _socketsTaskCompletions.TryAdd(id, taskCompletionSource);
        }

        public async Task RemoveSocket(string id)
        {
            WebSocket socket;
            _sockets.TryRemove(id, out socket);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the ConnectionManager",
                                    cancellationToken: CancellationToken.None);
        }

        internal void UpdateSocketKey(WebSocket socket, string newKey)
        {
            string id = GetId(socket);

            _sockets.TryRemove(id, out socket);
            _sockets.TryAdd(newKey, socket);

            TaskCompletionSource<object> taskCompletionSource;

            _socketsTaskCompletions.TryRemove(id, out taskCompletionSource);
            _socketsTaskCompletions.TryAdd(newKey, taskCompletionSource);
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}