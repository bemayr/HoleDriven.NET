using HoleDriven.Core;
using HoleDriven.Core.Logging;
using HoleDriven.Core.Reporters;
using HoleDriven.Core.Types;
using Microsoft.Extensions.Logging;

namespace HoleDriven
{
    [Hole.Idea("maybe Effect and Provide can be collapsed into Mock")]
    [Hole.Idea("maybe we can get rid of the async variations then (they would be automatically discoverd and async exection could be defined via a parameter)")]
    [Hole.Idea("provide the Mock attribute for partial methods, which would allow mock code generation (maybe with AutoBogus and C# 11's generic attributes)")]
    [Hole.Idea("integrate AutoBogus")]
    [Hole.Idea("switch configuration to a standard approach like https://github.com/nickdodd79/AutoBogus#conventions")]
    [Hole.Idea("enable **Markdown** in holes, e.g. using [Markdig](https://github.com/xoofx/markdig) for stripping out markdown while reporting the holes using the analyzer")]
    [Hole.Idea("add the HoleID as a scope while logging: https://blog.rsuter.com/logging-with-ilogger-recommendations-and-best-practices/#scopes")]
    public static partial class Hole
    {
        internal static IReportable Reporters => Core.Reporters.Reporters.Instance;
        internal static ILogger Logger { get;} = Dependencies.Instance.LoggerFactory.CreateLogger(typeof(Hole).FullName);

        private static string GetId(HoleLocation location)
        {
            // a hole here leads to a StackOverflow of course
            // TODO: "Id has to be generated based on the location, think of a generic way that is also compatible with Codegen",
            return $"{location.FileName}:{location.CallerMemberName}:line {location.LineNumber}";
        }
        private static void ReportHoleEncountered(HoleType type, string description, HoleLocation location)
        {
            Logger.LogDebug(
                HoleLogEvents.HoleEncountered,
                "Hole encountered {HoleId} {HoleType} {HoleDescription} {HoleLocation}",
                GetId(location),
                type,
                description,
                location);

            Reporters.InvokeHoleEncountered(type, location, description);
        }
    }
}
