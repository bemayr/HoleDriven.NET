using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentDate;
using HoleDriven.EffectHelpers;
using Spectre.Console;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class ProvideExample : IExample
    {
        public string Name => "Providing Values";
        public string Description => "Showing what Provide is for...";

        public void Execute()
        {
            var answer = Hole.Provide("we can simply provide a value and the type is inferred automatically", 42);
            AnsiConsole.WriteLine($"The answer is: {answer}");
        }
    }
}
