using Moq;
using System;
using System.Linq.Expressions;

namespace HoleDriven.Extension.Moq
{
    public static class ProvideValueExtensions
    {
        public static TMocked Moq<TMocked>(this Hole.IProvideInput _, Action<Mock<TMocked>> mock, MockBehavior mockBehavior = MockBehavior.Default)
            where TMocked : class
        {
            var mockedObject = new Mock<TMocked>(mockBehavior);
            mock(mockedObject);
            return mockedObject.Object;
        }

        public static TMocked Moq<TMocked>(this Hole.IProvideInput _, Expression<Func<TMocked, bool>> predicate, MockBehavior mockBehavior = MockBehavior.Default)
            where TMocked : class
        {
            return Mock.Of(predicate, mockBehavior);
        }
    }
}
