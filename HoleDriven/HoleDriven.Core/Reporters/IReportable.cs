using HoleDriven.Core.Types;

namespace HoleDriven.Core.Reporters
{
    internal interface IReportable
    {
        void InvokeFakeHappened(object value, HoleLocation location, string description);
        void InvokeHoleEncountered(HoleType type, HoleLocation location, string description);
    }
}