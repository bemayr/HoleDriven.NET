using HoleDriven.Core;
using HoleDriven.Extension.PrettyConsoleReporters;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class PrettyConsoleReportersExample : IExample
    {
        public string Name => "HoleDriven.Extension.PrettyConsoleReporters";
        public string Description => "Showing how the reporters can be customized...";

        [Hole.Idea("Provide Sections for Formatting")]
        public void Execute()
        {
            HoleDriven.Configure.Extensions.ActivatePrettyConsoleReporters(disable: Reporter.HoleEncountered);

            Hole.Refactor("some demo refactoring", () => "42");
            Hole.Idea("some idea");
            Hole.Provide("some valye for testing the pretty console reporters", 42);
            Hole.Effect("some demo effect for testing the pretty console reporters");

            HoleDriven.Configure.Reporters.RestoreDefaultReporters();
        }
    }
}
