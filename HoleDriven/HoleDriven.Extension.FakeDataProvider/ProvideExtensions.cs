using Bogus;
using System;

namespace HoleDriven.Bogus
{
    public static class ProvideValueExtensions
    {
        public static BogusProvider<TValue> Bogus<TValue>(this Hole.IProvideInput _, Func<Faker, TValue> fake)
        {
            return new BogusProvider<TValue>(fake(new Faker()));
        }

        public static BogusProvider<TValue> Bogus<TValue>(this Hole.IProvideInput _, Func<Faker<TValue>, Faker<TValue>> fake)
            where TValue : class
        {
            var value = fake(new Faker<TValue>()).Generate(); // TODO: why does this compile without Generate
            return new BogusProvider<TValue>(value);
        }
    }

    public class BogusProvider<TValue> : Hole.IProviderResult<TValue>
    {
        public BogusProvider(TValue value)
        {
            Value = value;
        }
        public TValue Value { get; }
    }
}
