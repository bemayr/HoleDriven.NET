using System;

namespace Holedriven
{
    public partial class Hole
    {
        public static TValue Get<TValue>(
            string description,
            TValue value)
        {
            Console.WriteLine($"Value provided: {value} ({description})");
            return value;
        }
    }
}
