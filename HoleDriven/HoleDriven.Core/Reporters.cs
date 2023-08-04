using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HoleDriven.Core
{
    public class Reporters
    {
        public delegate void HoleEncounteredDelegate(Core.HoleType type, string description, Core.HoleLocation location);
        public delegate void EffectHappenedDelegate(string description, Core.HoleLocation location);
        public delegate void EffectAsyncStartedDelegate(string description, Guid id, Task task, Core.HoleLocation location);
        public delegate void EffectAsyncCompletedDelegate(string description, Guid id, Task task, Core.HoleLocation location);
        public delegate void ProvideHappenedDelegate(string description, object value, Core.HoleLocation location);
        public delegate void ProvideAsyncStartedDelegate(string description, Guid id, Task task, Core.HoleLocation location);
        public delegate void ProvideAsyncCompletedDelegate(string description, object value, Guid id, Task task, Core.HoleLocation location);
        public delegate void ThrowHappenedDelegate(string description, Core.HoleNotFilledException exception, Core.HoleLocation location);

        public event HoleEncounteredDelegate HoleEncounteredReporter;
        public event EffectHappenedDelegate EffectHappenedReporter;
        public event EffectAsyncStartedDelegate EffectAsyncStartedReporter;
        public event EffectAsyncCompletedDelegate EffectAsyncCompletedReporter;
        public event ProvideHappenedDelegate ProvideHappenedReporter;
        public event ProvideAsyncStartedDelegate ProvideAsyncStartedReporter;
        public event ProvideAsyncCompletedDelegate ProvideAsyncCompletedReporter;
        public event ThrowHappenedDelegate ThrowHappenedReporter;

        public Reporters() => SetDefaultReporters();

        private void SetDefaultReporters()
        {
            HoleEncounteredReporter = Defaults.Reporter.HoleEncountered;
            EffectHappenedReporter = Defaults.Reporter.EffectHappened;
            EffectAsyncStartedReporter = Defaults.Reporter.EffectAsyncStarted;
            EffectAsyncCompletedReporter = Defaults.Reporter.EffectAsyncCompleted;
            ProvideHappenedReporter = Defaults.Reporter.ProvideHappened;
            ProvideAsyncStartedReporter = Defaults.Reporter.ProvideAsyncStarted;
            ProvideAsyncCompletedReporter = Defaults.Reporter.ProvideAsyncCompleted;
            ThrowHappenedReporter = Defaults.Reporter.ThrowHappened;
        }

        public void RemoveDefaultReporters()
        {
            RemoveDefaultHoleEncounteredReporter();
            RemoveDefaultEffectHappenedReporter();
            RemoveDefaultEffectAsyncStartedReporter();
            RemoveDefaultEffectAsyncCompletedReporter();
            RemoveDefaultProvideHappenedReporter();
            RemoveDefaultProvideAsyncStartedReporter();
            RemoveDefaultProvideAsyncCompletedReporter();
            RemoveDefaultThrowHappenedReporter();
        }
        public void RemoveDefaultHoleEncounteredReporter() => HoleEncounteredReporter -= Defaults.Reporter.HoleEncountered;
        public void RemoveDefaultEffectHappenedReporter() => EffectHappenedReporter -= Defaults.Reporter.EffectHappened;
        public void RemoveDefaultEffectAsyncStartedReporter() => EffectAsyncStartedReporter -= Defaults.Reporter.EffectAsyncStarted;
        public void RemoveDefaultEffectAsyncCompletedReporter() => EffectAsyncCompletedReporter -= Defaults.Reporter.EffectAsyncCompleted;
        public void RemoveDefaultProvideHappenedReporter() => ProvideHappenedReporter -= Defaults.Reporter.ProvideHappened;
        public void RemoveDefaultProvideAsyncStartedReporter() => ProvideAsyncStartedReporter -= Defaults.Reporter.ProvideAsyncStarted;
        public void RemoveDefaultProvideAsyncCompletedReporter() => ProvideAsyncCompletedReporter -= Defaults.Reporter.ProvideAsyncCompleted;
        public void RemoveDefaultThrowHappenedReporter() => ThrowHappenedReporter -= Defaults.Reporter.ThrowHappened;
        public void ReplaceReporters(
            HoleEncounteredDelegate holeEncounteredReporter,
            EffectHappenedDelegate effectHappenedReporter,
            EffectAsyncStartedDelegate effectAsyncStartedReporter,
            EffectAsyncCompletedDelegate effectAsyncCompletedReporter,
            ProvideHappenedDelegate provideHappenedReporter,
            ProvideAsyncStartedDelegate provideAsyncStartedReporter,
            ProvideAsyncCompletedDelegate provideAsyncCompletedReporter,
            ThrowHappenedDelegate throwHappenedReporter)
        {
            HoleEncounteredReporter = holeEncounteredReporter;
            EffectHappenedReporter = effectHappenedReporter;
            EffectAsyncStartedReporter = effectAsyncStartedReporter;
            EffectAsyncCompletedReporter = effectAsyncCompletedReporter;
            ProvideHappenedReporter = provideHappenedReporter;
            ProvideAsyncStartedReporter = provideAsyncStartedReporter;
            ProvideAsyncCompletedReporter = provideAsyncCompletedReporter;
            ThrowHappenedReporter = throwHappenedReporter;
        }
        public void ResetDefaultReporters() => SetDefaultReporters();
        public void DisableReporters(Reporter reporters)
        {
            if (reporters.HasFlag(Reporter.HoleEncountered)) HoleEncounteredReporter = null;
            if (reporters.HasFlag(Reporter.EffectHappened)) EffectHappenedReporter = null;
            if (reporters.HasFlag(Reporter.EffectAsyncStarted)) EffectAsyncStartedReporter = null;
            if (reporters.HasFlag(Reporter.EffectAsyncCompleted)) EffectAsyncCompletedReporter = null;
            if (reporters.HasFlag(Reporter.ProvideHappened)) ProvideHappenedReporter = null;
            if (reporters.HasFlag(Reporter.ProvideAsyncStarted)) ProvideAsyncStartedReporter = null;
            if (reporters.HasFlag(Reporter.ProvideAsyncCompleted)) ProvideAsyncCompletedReporter = null;
            if (reporters.HasFlag(Reporter.ThrowHappened)) ThrowHappenedReporter = null;
        }

        internal void HoleEncountered(string description, Core.HoleLocation location, [CallerMemberName] string callerMemberName = null) =>
            HoleEncounteredReporter?.Invoke((Core.HoleType)Enum.Parse(typeof(Core.HoleType), callerMemberName), description, location);
        internal void EffectHappened(string description, Core.HoleLocation location) =>
            EffectHappenedReporter?.Invoke(description, location);
        internal void EffectAsyncStarted(string description, Guid id, Task task, Core.HoleLocation location) =>
            EffectAsyncStartedReporter?.Invoke(description, id, task, location);
        internal void EffectAsyncCompleted(string description, Guid id, Task task, Core.HoleLocation location) =>
            EffectAsyncCompletedReporter?.Invoke(description, id, task, location);
        internal void ProvideHappened(string description, object value, Core.HoleLocation location) =>
            ProvideHappenedReporter?.Invoke(description, value, location);
        internal void ProvideAsyncStarted(string description, Guid id, Task task, Core.HoleLocation location) =>
            ProvideAsyncStartedReporter?.Invoke(description, id, task, location);
        internal void ProvideAsyncCompleted(string description, object value, Guid id, Task task, Core.HoleLocation location) =>
            ProvideAsyncCompletedReporter?.Invoke(description, value, id, task, location);
        internal void ThrowHappened(string description, Core.HoleNotFilledException exception, Core.HoleLocation location) =>
            ThrowHappenedReporter.Invoke(description, exception, location);
    }

    [Flags]
    public enum Reporter
    {
        None = 0,
        HoleEncountered = 1,
        EffectHappened = 2,
        EffectAsyncStarted = 4,
        EffectAsyncCompleted = 8,
        ProvideHappened = 16,
        ProvideAsyncStarted = 32,
        ProvideAsyncCompleted = 64,
        ThrowHappened = 128,
        All = ~None
    }

}
