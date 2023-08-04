using HoleDriven.Core;
using Spectre.Console;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class ThrowExample : IExample
    {
        public string Name => "Throwing Incomplete Holes";
        public string Description => "Showing what Hole.Throw is for...";

        public void Execute()
        {
            try
            {
                Hole.Throw("if we want to throw an exception upon encountering a Hole, we can simply use `Hole.Throw`");
            }
            catch (HoleNotFilledException exception)
            {
                AnsiConsole.WriteException(exception);
            }
        }
    }
}
