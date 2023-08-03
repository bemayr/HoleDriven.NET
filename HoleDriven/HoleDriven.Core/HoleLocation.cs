using System;
using System.Collections.Generic;
using System.Text;

namespace HoleDriven.Core
{
    public class HoleLocation
    {
        public HoleLocation(
            string filePath,
            int lineNumber,
            string callerMemberName)
        {
            FilePath = filePath;
            LineNumber = lineNumber;
            CallerMemberName = callerMemberName;
        }

        public string FilePath { get; }
        public int LineNumber { get; }
        public string CallerMemberName { get; }
    }
}
