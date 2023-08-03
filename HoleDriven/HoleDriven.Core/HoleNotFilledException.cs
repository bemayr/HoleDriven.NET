using System;

namespace HoleDriven.Core
{
    public class HoleNotFilledException : NotImplementedException
    {
        public HoleNotFilledException(string message) : base(message) { }

        // adapt the StackTrace in a way that the Throw method is invisible
        public override string StackTrace
        {
            get
            {
                int endOfFirstLine = base.StackTrace.IndexOf(Environment.NewLine);
                return base.StackTrace.Substring(endOfFirstLine + 1);
            }
        }
    }
}
