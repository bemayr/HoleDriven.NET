using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HoleDriven
{
    public static class Configuration
    {
        public static event Reporter.HoleEncounteredDelegate HoleEncounteredReporter;
        public static event Reporter.EffectHappenedDelegate EffectHappenedReporter;
        public static event Reporter.EffectAsyncStartedDelegate EffectAsyncStartedReporter;
        public static event Reporter.EffectAsyncCompletedDelegate EffectAsyncCompletedReporter;
        public static event Reporter.ProvideHappenedDelegate ProvideHappenedReporter;
        public static event Reporter.ProvideAsyncStartedDelegate ProvideAsyncStartedReporter;
        public static event Reporter.ProvideAsyncCompletedDelegate ProvideAsyncCompletedReporter;
        public static event Reporter.ThrowHappenedDelegate ThrowHappenedReporter;

        static Configuration()
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

        public static void RemoveDefaultReporters()
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
        public static void RemoveDefaultHoleEncounteredReporter() => HoleEncounteredReporter -= Defaults.Reporter.HoleEncountered;
        public static void RemoveDefaultEffectHappenedReporter() => EffectHappenedReporter -= Defaults.Reporter.EffectHappened;
        public static void RemoveDefaultEffectAsyncStartedReporter() => EffectAsyncStartedReporter -= Defaults.Reporter.EffectAsyncStarted;
        public static void RemoveDefaultEffectAsyncCompletedReporter() => EffectAsyncCompletedReporter -= Defaults.Reporter.EffectAsyncCompleted;
        public static void RemoveDefaultProvideHappenedReporter() => ProvideHappenedReporter -= Defaults.Reporter.ProvideHappened;
        public static void RemoveDefaultProvideAsyncStartedReporter() => ProvideAsyncStartedReporter -= Defaults.Reporter.ProvideAsyncStarted;
        public static void RemoveDefaultProvideAsyncCompletedReporter() => ProvideAsyncCompletedReporter -= Defaults.Reporter.ProvideAsyncCompleted;
        public static void RemoveDefaultThrowHappenedReporter() => ThrowHappenedReporter -= Defaults.Reporter.ThrowHappened;

        internal static void ReportHoleEncountered(string description, Core.HoleLocation location, [CallerMemberName] string callerMemberName = null) =>
            HoleEncounteredReporter?.Invoke((Core.HoleType)Enum.Parse(typeof(Core.HoleType), callerMemberName), description, location);
        internal static void ReportEffectHappened(string description, Core.HoleLocation location) =>
            EffectHappenedReporter?.Invoke(description, location);
        internal static void ReportEffectAsyncStarted(string description, Guid id, Task task, Core.HoleLocation location) =>
            EffectAsyncStartedReporter?.Invoke(description, id, task, location);
        internal static void ReportEffectAsyncCompleted(string description, Guid id, Task task, Core.HoleLocation location) =>
            EffectAsyncCompletedReporter?.Invoke(description, id, task, location);
        internal static void ReportProvideHappened(string description, object value, Core.HoleLocation location) =>
            ProvideHappenedReporter?.Invoke(description, value, location);
        internal static void ReportProvideAsyncStarted(string description, Guid id, Task task, Core.HoleLocation location) =>
            ProvideAsyncStartedReporter?.Invoke(description, id, task, location);
        internal static void ReportProvideAsyncCompleted(string description, object value, Guid id, Task task, Core.HoleLocation location) =>
            ProvideAsyncCompletedReporter?.Invoke(description, value, id, task, location);
        internal static void ReportThrowHappened(string description, Core.HoleNotFilledException exception, Core.HoleLocation location) =>
            ThrowHappenedReporter.Invoke(description, exception, location);
    }
}
