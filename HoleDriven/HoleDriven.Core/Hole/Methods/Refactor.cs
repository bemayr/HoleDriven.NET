using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace HoleDriven
{
    public static partial class Hole
    {
        public static void Refactor(
            string description,
            Expression<Action> expression, // TODO: maybe capture the expression via Caller...
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            Configuration.ReportHoleEncountered(description, location);
            expression.Compile()();
        }

        public static T Refactor<T>(
            string description,
            Expression<Func<T>> expression,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            Configuration.ReportHoleEncountered(description, location);
            return expression.Compile()();
        }
    }
}
