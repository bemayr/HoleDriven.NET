using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentDate;
using HoleDriven.EffectHelpers;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class EffectAsyncExample : IAsyncExample
    {
        public string Name => "Async Effects";
        public string Description => "Showing what Hole.EffectAsync is capable of...";

        public async Task ExecuteAsync()
        {
            await Hole.EffectAsync(
                "an async effect can only have a description, which leads to using Task.CompletedTask by default");

            await Hole.EffectAsync(
                "an async effect can supply a custom Task implementation",
                Task.Delay(1000));

            await Hole.EffectAsync(
                "an async effect can use metadata provided to the Task creation factory",
                effect => Task.Run(async () =>
                {
                    await Task.Delay(500);
                    await Console.Out.WriteLineAsync($"{ new { effect.Id, effect.Description } }");
                }));
        }
    }
}
