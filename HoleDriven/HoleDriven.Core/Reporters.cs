using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HoleDriven.Core
{
    public class Reporters
    {
        public event Reporter.HoleEncounteredDelegate HoleEncounteredReporter;
        public event Reporter.EffectHappenedDelegate EffectHappenedReporter;
        public event Reporter.EffectAsyncStartedDelegate EffectAsyncStartedReporter;
        public event Reporter.EffectAsyncCompletedDelegate EffectAsyncCompletedReporter;
        public event Reporter.ProvideHappenedDelegate ProvideHappenedReporter;
        public event Reporter.ProvideAsyncStartedDelegate ProvideAsyncStartedReporter;
        public event Reporter.ProvideAsyncCompletedDelegate ProvideAsyncCompletedReporter;
        public event Reporter.ThrowHappenedDelegate ThrowHappenedReporter;

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
            Reporter.HoleEncounteredDelegate holeEncounteredReporter,
            Reporter.EffectHappenedDelegate effectHappenedReporter,
            Reporter.EffectAsyncStartedDelegate effectAsyncStartedReporter,
            Reporter.EffectAsyncCompletedDelegate effectAsyncCompletedReporter,
            Reporter.ProvideHappenedDelegate provideHappenedReporter,
            Reporter.ProvideAsyncStartedDelegate provideAsyncStartedReporter,
            Reporter.ProvideAsyncCompletedDelegate provideAsyncCompletedReporter,
            Reporter.ThrowHappenedDelegate throwHappenedReporter)
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
}
