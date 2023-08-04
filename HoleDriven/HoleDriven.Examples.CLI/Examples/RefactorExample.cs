using HoleDriven.Core;
using Spectre.Console;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class RefactorExample : IExample
    {
        public string Name => "Signaling Refactorings";
        public string Description => "Showing what Hole.Refactor is for...";

        [Hole.Refactor("we can also use Hole.Refactor as an attribute, thus it does not get printed")]
        public void Execute()
        {
            var pi = Hole.Refactor(
                "we can also signal that something should be refactored, e.g. this literal should be replaced with a Math.PI",
                () => 3.1415);
            AnsiConsole.WriteLine($"PI is: {pi}");

            Hole.Refactor(
                "we can also mark some statement in a way that it needs refactoring, e.g. that the following secure PIN should be printed as ****",
                () => AnsiConsole.WriteLine("Pin Code: 0000"));
        }
    }
}
