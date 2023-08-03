using System;

namespace Holedriven
{
    public static partial class Hole
    {
        public static TValue Provide<TValue>(
            string description,
            TValue value)
        {
            ReportHole(nameof(Provide), description + $"Value provided: {value} ({description})");
            return value;
        }
    }
}
