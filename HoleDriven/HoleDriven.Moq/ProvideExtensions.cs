using Moq;
using System;
using System.Linq.Expressions;

namespace HoleDriven.Moq
{
    public static class ProvideValueExtensions
    {
        public static MoqProvider<TMocked> Moq<TMocked>(this Hole.IProvideInput _, Action<Mock<TMocked>> mock, MockBehavior mockBehavior = MockBehavior.Default)
            where TMocked : class
        {
            var mockedObject = new Mock<TMocked>(mockBehavior);
            mock(mockedObject);
            return new MoqProvider<TMocked>(mockedObject.Object);
        }

        public static MoqProvider<TMocked> Moq<TMocked>(this Hole.IProvideInput _, Expression<Func<TMocked, bool>> predicate, MockBehavior mockBehavior = MockBehavior.Default)
            where TMocked : class
        {
            return new MoqProvider<TMocked>(Mock.Of(predicate, MockBehavior.Strict));
        }
    }

    public class MoqProvider<TValue> : Hole.IProviderResult<TValue>
    {
        public MoqProvider(TValue value)
        {
            Value = value;
        }
        public TValue Value { get; }
    }
}
