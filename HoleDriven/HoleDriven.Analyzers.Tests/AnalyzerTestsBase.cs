using Xunit.Abstractions;

namespace HoleDriven.Analyzers.Tests
{
    public class HoleAnalyzerTestsBase
    {
        private readonly ITestOutputHelper output;

        public HoleAnalyzerTestsBase(ITestOutputHelper output)
        {
            this.output = output;
        }

        protected void Log(string message)
        {
            output.WriteLine(message);
        }
    }
}