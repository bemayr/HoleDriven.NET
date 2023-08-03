using System;
using System.Linq.Expressions;

namespace Holedriven
{
    public static partial class Hole
    {
        public static void Refactor(
            string description,
            Expression<Action> expression)
        {
            ReportHole(nameof(Refactor), description);
            expression.Compile()();
        }

        public static T Refactor<T>(
            string description,
            Expression<Func<T>> expression)
        {
            ReportHole(nameof(Refactor), description);
            return expression.Compile()();
        }
    }
}
