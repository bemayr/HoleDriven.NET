using HoleDriven.Core.Types;
using System;

namespace HoleDriven.Core.Reporters
{
    internal interface IReportable
    {
        void InvokeHoleEncountered(HoleInformation information);
        void InvokeFakeProvideHappened(HoleInformation information, object value);
        void InvokeFakeAsyncProvideStarted(HoleInformation information, Guid fakeId);
        void InvokeFakeAsyncProvideCompleted(HoleInformation information, Guid fakeId, object value);
        void InvokeFakeAsyncProvideFaulted(HoleInformation information, Guid fakeId, Exception exception);
        void InvokeFakeAsyncProvideCanceled(HoleInformation information, Guid fakeId);
        void InvokeFakeEffectHappened(HoleInformation information);
        void InvokeFakeAsyncEffectStarted(HoleInformation information, Guid fakeId);
        void InvokeFakeAsyncEffectCompleted(HoleInformation information, Guid fakeId);
        void InvokeFakeAsyncEffectFaulted(HoleInformation information, Guid fakeId, Exception exception);
        void InvokeFakeAsyncEffectCanceled(HoleInformation information, Guid fakeId);
    }
}