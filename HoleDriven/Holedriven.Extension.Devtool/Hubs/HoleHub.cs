using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Holedriven.Extension.Devtool.Hubs
{
    public class PromptHub : Hub
    {
        public void ProvideValue(Guid guid, string json)
        {
            PromptHelper.SignalResult(guid, json);
        }

        public override Task OnConnectedAsync()
        {
            PromptHelper.ClientConnected(Context.ConnectionId);
            Console.WriteLine("connected: " + Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            PromptHelper.ClientDisconnected(Context.ConnectionId);
            Console.WriteLine("disconnected: " + Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
