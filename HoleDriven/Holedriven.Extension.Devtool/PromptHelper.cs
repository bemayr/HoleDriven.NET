using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Holedriven.Extension.Devtool
{
    internal static class PromptHelper
    {
        private static readonly Dictionary<Guid, TaskCompletionSource<string>> _ongoingPrompts = new() { };
        public static ISet<string> ConnectedClients { get; } = new HashSet<string> { };

        public static bool CanPrompt => ConnectedClients.Count > 0;

        internal static TaskCompletionSource tcs { get; set; } = new TaskCompletionSource();
        public static Task UntilClientConnected => tcs.Task;

        public static void ClientConnected(string id)
        {
            ConnectedClients.Add(id);
            if (ConnectedClients.Count == 1)
                tcs.SetResult();
        }

        public static void ClientDisconnected(string id)
        {
            ConnectedClients.Remove(id);
        }


        internal static void AddPrompt(Guid guid)
        {
            _ongoingPrompts.Add(guid, new TaskCompletionSource<string>());
        }
        internal static void SignalResult(Guid guid, string result)
        {
            _ongoingPrompts[guid].SetResult(result);
        }
        internal static async Task<string> GetResult(Guid guid)
        {
            var result = await _ongoingPrompts[guid].Task;
            _ongoingPrompts.Remove(guid);
            return result;
        }
    }
}
