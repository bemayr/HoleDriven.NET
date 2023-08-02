using System;
using System.Linq.Expressions;

namespace Holedriven
{
    public partial class Hole
    {
        public static void Refactor(Expression<Action> expression)
        {
            var compiled = expression.Compile();
            throw new NotImplementedException();
        }

        public static void Refactor<T>(Expression<Func<T>> expression)
        {
            var compiled = expression.Compile();
            throw new NotImplementedException();
        }
    }
}
