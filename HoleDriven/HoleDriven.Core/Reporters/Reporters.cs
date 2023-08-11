using HoleDriven.Core.Types;
using System;

namespace HoleDriven.Core.Reporters
{
    public class Reporters : IReporters, IReportable
    {
        private static readonly Lazy<Reporters> lazy = new Lazy<Reporters>(() => new Reporters());
        public static Reporters Instance => lazy.Value;

        public event HoleEncounteredDelegate HoleEncountered;
        public event FakeProvideHappenedDelegate FakeProvideHappened;
        public event FakeAsyncProvideStartedDelegate FakeAsyncProvideStarted;
        public event FakeAsyncProvideCompletedDelegate FakeAsyncProvideCompleted;
        public event FakeAsyncProvideFaultedDelegate FakeAsyncProvideFaulted;
        public event FakeAsyncProvideCanceledDelegate FakeAsyncProvideCanceled;
        public event FakeEffectHappenedDelegate FakeEffectHappened;
        public event FakeAsyncEffectStartedDelegate FakeAsyncEffectStarted;
        public event FakeAsyncEffectCompletedDelegate FakeAsyncEffectCompleted;
        public event FakeAsyncEffectFaultedDelegate FakeAsyncEffectFaulted;
        public event FakeAsyncEffectCanceledDelegate FakeAsyncEffectCanceled;

        private Reporters() { }

        public void InvokeHoleEncountered(HoleInformation information) => HoleEncountered?.Invoke(information);
        public void InvokeFakeProvideHappened(HoleInformation information, object value) => FakeProvideHappened?.Invoke(information, value);
        public void InvokeFakeAsyncProvideStarted(HoleInformation information, Guid fakeId) => FakeAsyncProvideStarted?.Invoke(information, fakeId);
        public void InvokeFakeAsyncProvideCompleted(HoleInformation information, Guid fakeId, object value) => FakeAsyncProvideCompleted(information, fakeId, value);
        public void InvokeFakeAsyncProvideFaulted(HoleInformation information, Guid fakeId, Exception exception) => FakeAsyncProvideFaulted?.Invoke(information, fakeId, exception);
        public void InvokeFakeAsyncProvideCanceled(HoleInformation information, Guid fakeId) => FakeAsyncProvideCanceled?.Invoke(information, fakeId);
        public void InvokeFakeEffectHappened(HoleInformation information) => FakeEffectHappened?.Invoke(information);
        public void InvokeFakeAsyncEffectStarted(HoleInformation information, Guid fakeId) => FakeAsyncEffectStarted(information, fakeId);
        public void InvokeFakeAsyncEffectCompleted(HoleInformation information, Guid fakeId) => FakeAsyncEffectCompleted(information, fakeId);
        public void InvokeFakeAsyncEffectFaulted(HoleInformation information, Guid fakeId, Exception exception) => FakeAsyncEffectFaulted?.Invoke(information, fakeId, exception);
        public void InvokeFakeAsyncEffectCanceled(HoleInformation information, Guid fakeId) => FakeAsyncEffectCanceled?.Invoke(information, fakeId);

        public delegate void HoleEncounteredDelegate(HoleInformation information);
        public delegate void FakeProvideHappenedDelegate(HoleInformation information, object value);
        public delegate void FakeAsyncProvideStartedDelegate(HoleInformation information, Guid fakeId);
        public delegate void FakeAsyncProvideCompletedDelegate(HoleInformation information, Guid fakeId, object value);
        public delegate void FakeAsyncProvideFaultedDelegate(HoleInformation information, Guid fakeId, Exception exception);
        public delegate void FakeAsyncProvideCanceledDelegate(HoleInformation information, Guid fakeId);
        public delegate void FakeEffectHappenedDelegate(HoleInformation information);
        public delegate void FakeAsyncEffectStartedDelegate(HoleInformation information, Guid fakeId);
        public delegate void FakeAsyncEffectCompletedDelegate(HoleInformation information, Guid fakeId);
        public delegate void FakeAsyncEffectFaultedDelegate(HoleInformation information, Guid fakeId, Exception exception);
        public delegate void FakeAsyncEffectCanceledDelegate(HoleInformation information, Guid fakeId);

    }
}
