using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoleDriven.Core.Logging
{
    internal static class HoleLogEvents
    {
        internal static EventId HoleEncountered = new EventId(1000, nameof(HoleEncountered));
        internal static EventId FakeProvideHappened = new EventId(1100, nameof(FakeProvideHappened));
        internal static EventId FakeAsyncProvideStarted = new EventId(1101, nameof(FakeAsyncProvideStarted));
        internal static EventId FakeAsyncProvideCompleted = new EventId(1102, nameof(FakeAsyncProvideCompleted));
        internal static EventId FakeAsyncProvideFaulted = new EventId(1103, nameof(FakeAsyncProvideFaulted));
        internal static EventId FakeAsyncProvideCanceled = new EventId(1104, nameof(FakeAsyncProvideCanceled));
        internal static EventId FakeEffectHappened = new EventId(1110, nameof(FakeProvideHappened));
        internal static EventId FakeAsyncEffectStarted = new EventId(1111, nameof(FakeAsyncEffectStarted));
        internal static EventId FakeAsyncEffectCompleted = new EventId(1112, nameof(FakeAsyncEffectCompleted));
        internal static EventId FakeAsyncEffectFaulted = new EventId(1113, nameof(FakeAsyncEffectFaulted));
        internal static EventId FakeAsyncEffectCanceled = new EventId(1114, nameof(FakeAsyncEffectCanceled));
    }
}
