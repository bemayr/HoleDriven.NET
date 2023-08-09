using HoleDriven.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static HoleDriven.Hole;

namespace HoleDriven
{
    public static partial class Hole
    {
        public delegate TValue ProvideValueProvider<TValue>(IProvideInput hole);

        public interface IProvideInput
        {
            string Description { get; }
        }

        internal class ProvideInput : IProvideInput
        {
            public string Description { get; }
            public ProvideInput(string description) => Description = description;
        }

        public static TValue Provide<TValue>(
            string description,
            TValue value,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            ReportHoleEncountered(nameof(Provide), description, location);
            ReportProvideHappened(description, value, location);
            return value;
        }

        public static TValue Provide<TValue>(
            string description,
            ProvideValueProvider<TValue> valueProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            ReportHoleEncountered(nameof(Provide), description, location);
            var value = valueProvider(new ProvideInput(description));
            ReportProvideHappened(description, value, location);
            return value;
        }

        private static void ReportProvideHappened(string description, object value, Core.HoleLocation location)
        {
            Console.WriteLine("PROVIDE HAPPENED");
            Logger.LogInformation(
                HoleLogEvents.HoleEncountered,
                "Provide happened {HoleId} {value} {HoleType} {HoleDescription} {HoleLocation}",
                GetId(location),
                value,
                nameof(Provide),
                description,
                location);

            Report.ProvideHappened(description, value, location);
        }
    }
}
