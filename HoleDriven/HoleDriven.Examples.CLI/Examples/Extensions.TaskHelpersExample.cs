using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentDate;
using HoleDriven.Core;
using HoleDriven.EffectHelpers;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class TaskHelpersExample : IAsyncExample
    {
        public string Name => "HoleDriven.Extension.TaskHelpers";
        public string Description => "Showing what HoleDriven.Extension.TaskHelpers is capable of...";

        public async Task ExecuteAsync()
        {
            await Hole.EffectAsync(
                "TaskHelpers references FluentDateTime which enables natural language TimeSpan definitions",
                Task.Delay(42.Milliseconds()));

            await Hole.EffectAsync(
                "TaskHelpers provides <Task>.WithProbabilities(success, cancelled, errored) which allows dynamic Task mocking",
                Task.Delay(100.Milliseconds()).WithProbabilities(success: 0.4, cancelled: 0.3, errored: 0.3));
        }
    }
}
