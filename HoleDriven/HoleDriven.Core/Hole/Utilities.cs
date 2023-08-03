using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace HoleDriven
{
    public static partial class Hole
    {
        public delegate void HoleReporterDelegate(string hole, string description, Core.HoleLocation location);

        public static HoleReporterDelegate HoleReporter { get; internal set; }

        static Hole()
        {
            HoleReporter = DefaultConfiguration.ReportHole;
        }

        // local utility methods that forward to the configured effects
        internal static void ReportHole(
            string description,
            Core.HoleLocation location,
            [CallerMemberName] string callerMemberName = null) => HoleReporter.Invoke(callerMemberName, description, location);

    }

    public static class Configure
    {
        public static void Reporter(Hole.HoleReporterDelegate holeReporter)
        {
            Hole.HoleReporter = holeReporter;
        }
    }

    public static class DefaultConfiguration
    {
        public static void ReportHole(string hole, string description, Core.HoleLocation location)
        {
            Console.WriteLine($"[🧩 {hole}]: {description} (at {FormatLocation(location)})");
        }
        private static string FormatLocation(Core.HoleLocation location)
        {
            var fileName = Path.GetFileName(location.FilePath);
            return $"{location.CallerMemberName} in {fileName}:line {location.LineNumber}";
        }
    }
}
