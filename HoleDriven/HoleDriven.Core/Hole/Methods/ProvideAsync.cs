using System;
using System.Threading.Tasks;

namespace Holedriven
{
    public static partial class Hole
    {
        public static Task<TValue> ProvideAsync<TValue>(
            string description,
            Task<TValue> value)
        {
            ReportHole(nameof(ProvideAsync), description + $"Value provided asynchronously: {value} ({description})");
            return value;
        }
    }
}
