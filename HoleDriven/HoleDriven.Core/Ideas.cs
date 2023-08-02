using System;
using System.ComponentModel;

namespace Holedriven
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentInterface
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }

    public partial class Hole : IFluentInterface
    {
        //public static void Todo([Hole.Enum("test")] string message, [Hole.Interface] ICollected collected)
        //{
        //    Hole.Refactor(
        //        () => Console.WriteLine(message),
        //        "use ILogger interface");
        //}
    }
}
