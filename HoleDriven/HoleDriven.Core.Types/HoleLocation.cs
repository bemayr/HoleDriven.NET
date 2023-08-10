using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HoleDriven.Core.Types
{
    public class HoleLocation
    {
        private readonly Lazy<string> _fileName;

        public HoleLocation(
            string filePath,
            int lineNumber,
            string callerMemberName)
        {
            FilePath = filePath;
            _fileName = new Lazy<string>(() => Path.GetFileName(FilePath));
            LineNumber = lineNumber;
            CallerMemberName = callerMemberName;
        }

        public string FilePath { get; }
        public string FileName => _fileName.Value;
        public int LineNumber { get; }
        public string CallerMemberName { get; }

        public override string ToString() => $"{CallerMemberName} in {FileName}:line {LineNumber}";
    }
}
