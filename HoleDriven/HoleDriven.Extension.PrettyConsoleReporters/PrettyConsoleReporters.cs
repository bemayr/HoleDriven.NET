using HoleDriven;
using HoleDriven.Core;
using Spectre.Console;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HoleDriven.Extension.PrettyConsoleReporters
{
    [Hole.Refactor("Make sure that the descriptions are properly escaped, otherwise we might run into problems")]
    public static class Extension
    {
        [Hole.Idea("add an enum flag that allows disabling of individual reporters")]
        [Hole.Idea("add a setting that disables or enables style interpolation")]
        public static void ActivatePrettyConsoleReporters(this Core.IExtensionMarker _, Reporter disable = Reporter.None)
        {
            Configure.Reporters.ReplaceReporters(HoleEncountered, EffectHappened, EffectAsyncStarted, EffectAsyncCompleted, ProvideHappened, ProvideAsyncStarted, ProvideAsyncCompleted, ThrowHappened);
            Configure.Reporters.DisableReporters(disable);
        }

        private static void HoleEncountered(Core.HoleType type, string description, Core.HoleLocation location) =>
            AnsiConsole.MarkupLine($"🧩 [bold invert springgreen1][[{type}]][/]: {description} [dim](at {FormatLocation(location)})[/]");
        private static void EffectHappened(string description, Core.HoleLocation location) =>
            AnsiConsole.MarkupLine($"🧩🥏 [bold invert turquoise2][[EFFECT]][/]: {description} [dim](at {FormatLocation(location)})[/]");
        private static void EffectAsyncStarted(string description, Guid id, Task task, Core.HoleLocation location) =>
            Hole.Effect("⚒️ Log EffectAsyncStarted");
        private static void EffectAsyncCompleted(string description, Guid id, Task task, Core.HoleLocation location) =>
            Hole.Effect("⚒️ Log EffectAsyncCompleted");
        private static void ProvideHappened(string description, object value, Core.HoleLocation location) =>
            AnsiConsole.MarkupLine($"🧩📤 [bold invert slateblue1][[PROVIDE]][/]: providing '{value}' [dim](at {FormatLocation(location)})[/]");
        private static void ProvideAsyncStarted(string description, Guid id, Task task, Core.HoleLocation location) =>
            Hole.Effect("⚒️ Log ProvideAsyncStarted");
        private static void ProvideAsyncCompleted(string description, object value, Guid id, Task task, Core.HoleLocation location) =>
            Hole.Effect("⚒️ Log ProvideAsyncCompleted");
        private static void ThrowHappened(string description, Core.HoleNotFilledException exception, Core.HoleLocation location)
        {
            AnsiConsole.MarkupLine($"🧩💣 [bold invert red][[THROW]][/] [dim](at {FormatLocation(location)})[/]");
            AnsiConsole.WriteException(exception);
        }

        private static string FormatLocation(Core.HoleLocation location) => location.ToString();
    }
}
