using System;
using System.Diagnostics;

namespace Holedriven
{
    public partial class Hole
    {
        [DebuggerHidden]
        public static void Throw(string description)
        {
            throw new NotImplementedException(description);
        }
    }
}
