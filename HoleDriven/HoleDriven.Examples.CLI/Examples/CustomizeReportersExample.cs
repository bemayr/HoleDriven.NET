using Spectre.Console;

namespace HoleDriven.Examples.CLI.Examples
{
    internal class CustomizeReportersExample : IExample
    {
        public string Name => "Customize the Reporters";
        public string Description => "Showing how the reporters can be customized...";

        [Hole.Idea("Provide Sections for Formatting")]
        public void Execute()
        {
            AnsiConsole.WriteLine("=== using the default reporters ===");
            Hole.Effect("some demo effect for testing the reporters");

            AnsiConsole.WriteLine("=== removing the hole encountered reporter ===");
            HoleDriven.Configure.Reporters.RemoveDefaultHoleEncounteredReporter();
            Hole.Effect("some demo effect for testing the reporters");
            HoleDriven.Configure.Reporters.RestoreDefaultReporters();

            AnsiConsole.WriteLine("=== customizing the effect happened reporter ===");
            HoleDriven.Configure.Reporters.EffectHappenedReporter += (description, location) =>
            {
                var fileName = Path.GetFileName(location.FilePath);
                var formattedLocation = $"{location.CallerMemberName} in {fileName}:line {location.LineNumber}";
                AnsiConsole.MarkupLine($"🧩 [bold invert turquoise2][[EFFECT 🥏]][/]: [italic]{description}[/] [dim](at {formattedLocation})[/]");
            };
            Hole.Effect("some demo effect for testing the reporters");
            HoleDriven.Configure.Reporters.RestoreDefaultReporters();

            AnsiConsole.WriteLine("=== removing the hole encountered reporter ===");
            HoleDriven.Configure.Reporters.DisableReporters(Core.Reporter.EffectHappened);
            Hole.Effect("some demo effect for testing the reporters");

            AnsiConsole.WriteLine("=== restoring the default reporters ===");
            HoleDriven.Configure.Reporters.RestoreDefaultReporters();
        }
    }
}
