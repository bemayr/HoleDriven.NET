using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using Holedriven.Extension.Devtool.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Holedriven.Extension.Devtool
{
    internal class Devtool
    {
        private Devtool(
            Uri frontendUri,
            Uri proxyUri,
            IHubContext<PromptHub> promptsHub)
        {
            FrontendUri = frontendUri;
            ProxyUri = proxyUri;
            PromptsHub = promptsHub;
            Uri = new Uri(QueryHelpers.AddQueryString(
                FrontendUri.AbsoluteUri,
                new Dictionary<string, string>() { { "connection", ProxyUri.AbsoluteUri } }));
        }

        public Uri FrontendUri { get; }
        public Uri ProxyUri { get; }
        public Uri Uri { get; }
        public IHubContext<PromptHub> PromptsHub { get; }
        internal static Devtool Instance { get; private set; }

        internal static void Create(
            Uri frontendUri,
            Uri proxyUri,
            IHubContext<PromptHub> promptsHub)
        {
            Debug.Assert(Instance == null, "we can not create the Devtool instance twice");
            Instance = new Devtool(frontendUri, proxyUri, promptsHub);
        }

        public void OpenFrontend() => new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = Uri.AbsoluteUri
            }
        }.Start();
    }
}
