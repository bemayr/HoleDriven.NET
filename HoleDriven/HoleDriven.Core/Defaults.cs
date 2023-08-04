using System;
using System.IO;
using System.Threading.Tasks;

namespace HoleDriven
{
    public static class Reporter
    {
        public delegate void HoleEncounteredDelegate(Core.HoleType type, string description, Core.HoleLocation location);
        public delegate void EffectHappenedDelegate(string description, Core.HoleLocation location);
        public delegate void EffectAsyncStartedDelegate(string description, Guid id, Task task, Core.HoleLocation location);
        public delegate void EffectAsyncCompletedDelegate(string description, Guid id, Task task, Core.HoleLocation location);
        public delegate void ProvideHappenedDelegate(string description, object value, Core.HoleLocation location);
        public delegate void ProvideAsyncStartedDelegate(string description, Guid id, Task task, Core.HoleLocation location);
        public delegate void ProvideAsyncCompletedDelegate(string description, object value, Guid id, Task task, Core.HoleLocation location);
        public delegate void ThrowHappenedDelegate(string description, Core.HoleNotFilledException exception, Core.HoleLocation location);
    }

    public static class Defaults
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "these parameters must conform with the respective delegates")]
        public static class Reporter
        {
            // TODO: maybe provide some additional hole information (for Refactor, Idea, ...)
            public static void HoleEncountered(Core.HoleType type, string description, Core.HoleLocation location) =>
                Console.WriteLine($"[🧩 {type}]: {description} (at {Core.Utilities.FormatLocation(location)})");
            public static void EffectHappened(string description, Core.HoleLocation location) =>
                Console.WriteLine($"[🥏 EFFECT]: {description} (at {Core.Utilities.FormatLocation(location)})");
            public static void EffectAsyncStarted(string description, Guid id, Task task, Core.HoleLocation location) =>
                Console.WriteLine($"[🥏 EFFECT.ASYNC] Started({id}): {description} (at {Core.Utilities.FormatLocation(location)})");
            public static void EffectAsyncCompleted(string description, Guid id, Task task, Core.HoleLocation location) =>
                Console.WriteLine($"[🥏 EFFECT.ASYNC] Completed({id}, {task.Status}): {description} (at {Core.Utilities.FormatLocation(location)})");
            public static void ProvideHappened(string description, object value, Core.HoleLocation location) =>
                Console.WriteLine($"[📤 PROVIDE]: Value provided: '{value}' (at {Core.Utilities.FormatLocation(location)})");
            public static void ProvideAsyncStarted(string description, Guid id, Task task, Core.HoleLocation location) =>
                Console.WriteLine($"[📤 PROVIDE.ASYNC] Started({id}) (at {Core.Utilities.FormatLocation(location)})");
            public static void ProvideAsyncCompleted(string description, object value, Guid id, Task task, Core.HoleLocation location) =>
                Console.WriteLine($"[📤 PROVIDE.ASYNC] Completed({id}, {task.Status}) with value '{value}' (at {Core.Utilities.FormatLocation(location)})");
            public static void ThrowHappened(string description, Core.HoleNotFilledException exception, Core.HoleLocation location) =>
                Console.WriteLine($"[💣 THROW]: {description} (at {Core.Utilities.FormatLocation(location)})");
        }
    }
}

namespace HoleDriven.Core
{
    public enum HoleType
    {
        Refactor,
        Idea,
        Effect,
        EffectAsync,
        Provide,
        ProvideAsync,
        Throw
    }

    internal static class Utilities
    {
        internal static string FormatLocation(HoleLocation location)
        {
            var fileName = Path.GetFileName(location.FilePath);
            return $"{location.CallerMemberName} in {fileName}:line {location.LineNumber}";
        }
    }
}