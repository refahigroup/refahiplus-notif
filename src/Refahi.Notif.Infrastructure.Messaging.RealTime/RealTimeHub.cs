using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Refahi.Notif.Infrastructure.Messaging.RealTime
{
    //[Authorize]
    public class RealTimeHub : Hub
    {
        IHttpContextAccessor _http;
        readonly ILogger<RealTimeHub> _logger;
        private static readonly ConcurrentDictionary<string, DateTime> ConnectedUsers = new ConcurrentDictionary<string, DateTime>();
        private static int _totalConnections = 0;
        private static int _totalDisconnections = 0;
        public RealTimeHub(IHttpContextAccessor http, ILogger<RealTimeHub> logger)
        {
            _http = http;
            _logger = logger;
        }


        public override async Task OnConnectedAsync()
        {
            ConnectedUsers.TryAdd(Context.ConnectionId, DateTime.UtcNow);
            Interlocked.Increment(ref _totalConnections);
            _logger.LogInformation("User Connected: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
            UpdateActiveUserCount();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            DateTime removed;
            ConnectedUsers.TryRemove(Context.ConnectionId, out removed);
            Interlocked.Increment(ref _totalDisconnections);
            _logger.LogInformation("User Disconnected: {ConnectionId} Duration: {duration}", new object?[]
            {
                Context.ConnectionId,
                DateTime.UtcNow.Subtract(removed).TotalSeconds
            });
            await base.OnDisconnectedAsync(exception);
            UpdateActiveUserCount();
        }

        public async Task SendMessage(string[] connectionIds, string type, string message)
        {
            await Clients?.Clients(connectionIds)?.SendAsync("message", type, message);
        }

        public async Task SendMessageToUser(long userId, string type, string message)
        {
            if (Clients == null) return;
            var user = Clients.User(userId.ToString());
            if (user == null) return;
            await user.SendAsync("message", type, message);
        }
        private void UpdateActiveUserCount()
        {
            _logger.LogInformation("UpdateActiveUserCount: {connectedUsers}", ConnectedUsers.Count);
        }

    }

}
