using HoleDriven;
using Spectre.Console;
using System.IO;

namespace HoleDriven.Extension.PrettyConsoleReporters
{
    public static class Extension
    {
        public static void ActivatePrettyConsoleReporters(this Core.Extensions _)
        {
            Configure.Reporters.RemoveDefaultReporters();
            Configure.Reporters.HoleEncounteredReporter += (type, description, location) =>
                AnsiConsole.MarkupLine($"🧩 [bold invert springgreen1][[{type}]][/]: {description} [dim](at {FormatLocation(location)})[/]");
            Configure.Reporters.EffectHappenedReporter += (description, location) =>
                AnsiConsole.MarkupLine($"🧩🥏 [bold invert turquoise2][[EFFECT]][/]: {description} [dim](at {FormatLocation(location)})[/]");
            Configure.Reporters.ProvideHappenedReporter += (description, value, location) =>
                AnsiConsole.MarkupLine($"🧩📤 [bold invert slateblue1][[PROVIDE]][/]: providing '{value}' [dim](at {FormatLocation(location)})[/]");
            Configure.Reporters.ThrowHappenedReporter += (description, exception, location) =>
            {
                AnsiConsole.MarkupLine($"🧩💣 [bold invert red][[THROW]][/] [dim](at {FormatLocation(location)})[/]");
                AnsiConsole.WriteException(exception);
            };
        }

        private static string FormatLocation(Core.HoleLocation location) => location.ToString();
    }
}
