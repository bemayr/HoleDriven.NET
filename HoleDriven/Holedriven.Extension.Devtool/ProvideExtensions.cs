using HoleDriven;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Holedriven.Extension.Devtool
{

    public static class ProvideExtensions
    {
        public static TValue Prompt<TValue>(this Hole.IProvideInput _)
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
                Console.WriteLine("⚠️ ⏸️ Please connect a Client for prompting!");
                //Devtool.Instance.OpenFrontend();
                await PromptHelper.UntilClientConnected;
            }

            var generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(TValue));

            var schemaJson = schema.ToString();

            var guid = Guid.NewGuid();

            PromptHelper.AddPrompt(guid);

            await Devtool.Instance.PromptsHub.Clients.All.SendAsync("prompt", new { guid, schemaJson });
            Console.WriteLine("prompt sent");

            // wait for result
            var json = await PromptHelper.GetResult(guid);

            var value = JsonConvert.DeserializeObject<TValue>(json);

            //var value = default(TValue);
            return value;
        }
    }
}
