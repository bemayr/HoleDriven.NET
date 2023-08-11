using HoleDriven.Core.Types;
using static HoleDriven.Core.Reporters.Reporters;

namespace HoleDriven.Core.Reporters
{
    public interface IReporters
    {
        event HoleEncounteredDelegate HoleEncountered;
        event FakeProvideHappenedDelegate FakeProvideHappened;
        event FakeAsyncProvideStartedDelegate FakeAsyncProvideStarted;
        event FakeAsyncProvideCompletedDelegate FakeAsyncProvideCompleted;
        event FakeAsyncProvideFaultedDelegate FakeAsyncProvideFaulted;
        event FakeAsyncProvideCanceledDelegate FakeAsyncProvideCanceled;
        event FakeEffectHappenedDelegate FakeEffectHappened;
        event FakeAsyncEffectStartedDelegate FakeAsyncEffectStarted;
        event FakeAsyncEffectCompletedDelegate FakeAsyncEffectCompleted;
        event FakeAsyncEffectFaultedDelegate FakeAsyncEffectFaulted;
        event FakeAsyncEffectCanceledDelegate FakeAsyncEffectCanceled;
    }
}