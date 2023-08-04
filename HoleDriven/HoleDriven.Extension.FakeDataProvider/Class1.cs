using Bogus;
using System;
using System.Threading.Tasks;

namespace HoleDriven.Extension.FakeDataProvider
{
    public static class ProvideValueExtensions
    {
        public static FakerResult<TValue> Fake<TValue>(this Hole.IProvideInput _, Func<Faker, TValue> fake)
        {
            return new FakerResult<TValue>(fake(new Faker()));
        }

        public static FakerResult<TValue> Fake<TValue>(this Hole.IProvideInput _, Func<Faker<TValue>, Faker<TValue>> fake)
            where TValue : class
        {
            var value = fake(new Faker<TValue>()).Generate(); // TODO: why does this compile without Generate
            return new FakerResult<TValue>(value);
        }
    }

    public class FakerResult<TValue> : Hole.IProviderResult<TValue>
    {
        public FakerResult(TValue value)
        {
            Value = value;
        }
        public TValue Value { get; }
    }
}
