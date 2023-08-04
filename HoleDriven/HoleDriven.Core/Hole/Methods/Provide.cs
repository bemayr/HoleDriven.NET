using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static HoleDriven.Hole;

namespace HoleDriven
{
    public static partial class Hole
    {
        public interface IProviderResult<TValue>
        {
            TValue Value { get; }
        }

        public delegate IProviderResult<TValue> ProvideValueProvider<TValue>(IProvideInput hole);

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
            Configuration.ReportHoleEncountered(description, location);
            Configuration.ReportProvideHappened(description, value, location);
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
            var value = valueProvider(new ProvideInput(description)).Value;

            Configuration.ReportHoleEncountered(description, location);
            Configuration.ReportProvideHappened(description, value, location);
            return value;
        }
    }
}
