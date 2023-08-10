using HoleDriven.Core.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Holedriven.Extension.Devtool.Hubs
{
    public class PromptHub : Hub
    {
        private readonly ILogger<PromptHub> logger;

        public PromptHub(ILogger<PromptHub> logger)
        {
            this.logger = logger;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "this is needed for SignalR, but should be solved once the Hub is not a static member anymore")]
        public void ProvideValue(Guid guid, string json)
        {
            PromptHelper.SignalResult(guid, json);
        }

        public override Task OnConnectedAsync()
        {
            PromptHelper.ClientConnected(Context.ConnectionId);
            logger.LogInformation(DevtoolLogEvents.ClientConnected, "Client connected: {ConnectionId}", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            PromptHelper.ClientDisconnected(Context.ConnectionId);
            logger.LogInformation(DevtoolLogEvents.ClientDisconnected, "Client disconnected: {ConnectionId}", Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
