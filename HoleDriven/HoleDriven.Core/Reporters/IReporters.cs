using HoleDriven.Core.Types;

namespace HoleDriven.Core.Reporters
{
    public interface IReporters
    {
        event FakeHappened.Delegate FakeHappened;
        event HoleEncountered.Delegate HoleEncountered;
    }
}