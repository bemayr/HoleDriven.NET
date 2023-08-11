using HoleDriven.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Threading.Tasks;

namespace Holedriven.Extension.Devtool
{
    [Hole.Refactor("Improve the usage of Dependencies in the static methods, they should somehow be provided via the extension entrypoint")]
    public static class ProvideExtensions
    {
        public static TValue Prompt<TValue>(this Hole.IFakeExtension _)
        {
            return PromptAsync<TValue>()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        private async static Task<TValue> PromptAsync<TValue>()
        {
            if (!PromptHelper.CanPrompt)
            {
                // maybe use loggerFactory passed into extension and make the dependencies instance internal
                var logger = Dependencies.Instance.LoggerFactory.CreateLogger(typeof(ProvideExtensions).FullName);
                logger.LogWarning("No client was connect that can handle prompts...");
                //Devtool.Instance.OpenFrontend();
                await PromptHelper.UntilClientConnected;
            }

            var generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(TValue));

            var schemaJson = schema.ToString();

            var guid = Guid.NewGuid();

            PromptHelper.AddPrompt(guid);

            await Devtool.Instance.PromptsHub.Clients.All.SendAsync("prompt", new { guid, schemaJson });

            // wait for result
            var json = await PromptHelper.GetResult(guid);

            var value = JsonConvert.DeserializeObject<TValue>(json);

            //var value = default(TValue);
            return value;
        }
    }
}
