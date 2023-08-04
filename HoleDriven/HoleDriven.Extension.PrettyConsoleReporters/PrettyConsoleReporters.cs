using HoleDriven.Core;
using Spectre.Console;
using System.IO;

namespace HoleDriven.Extension
{
    public static class PrettyConsoleReporters
    {
        public static void Activate()
        {
            Configuration.RemoveDefaultReporters();
            Configuration.HoleEncounteredReporter += (type, description, location) =>
                AnsiConsole.MarkupLine($"🧩 [bold invert springgreen1][[{type}]][/]: {description} [dim](at {FormatLocation(location)})[/]");
            Configuration.EffectHappenedReporter += (description, location) =>
                AnsiConsole.MarkupLine($"🧩🥏 [bold invert turquoise2][[EFFECT]][/]: {description} [dim](at {FormatLocation(location)})[/]");
            Configuration.ProvideHappenedReporter += (description, value, location) =>
                AnsiConsole.MarkupLine($"🧩📤 [bold invert slateblue1][[PROVIDE]][/]: providing '{value}' [dim](at {FormatLocation(location)})[/]");
            Configuration.ThrowHappenedReporter += (description, exception, location) =>
            {
                AnsiConsole.MarkupLine($"🧩💣 [bold invert red][[THROW]][/] [dim](at {FormatLocation(location)})[/]");
                AnsiConsole.WriteException(exception);
            };
        }
        internal static string FormatLocation(HoleLocation location) => location.ToString();
    }
}
