using HoleDriven.Core.Logging;
using HoleDriven.Core.Reporters;
using HoleDriven.Core.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HoleDriven.Core
{
    [Idea("maybe Effect and Provide can be collapsed into Mock")]
    [Idea("maybe we can get rid of the async variations then (they would be automatically discoverd and async exection could be defined via a parameter)")]
    [Idea("provide the Mock attribute for partial methods, which would allow mock code generation (maybe with AutoBogus and C# 11's generic attributes)")]
    [Idea("integrate AutoBogus")]
    [Idea("switch configuration to a standard approach like https://github.com/nickdodd79/AutoBogus#conventions")]
    [Idea("enable **Markdown** in holes, e.g. using [Markdig](https://github.com/xoofx/markdig) for stripping out markdown while reporting the holes using the analyzer")]
    [Idea("add the HoleID as a scope while logging: https://blog.rsuter.com/logging-with-ilogger-recommendations-and-best-practices/#scopes")]
    
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Parameters are extracted via Codegen and used lazily via Reflection at Runtime")]
    public static partial class Hole
    {
        internal static IReportable Reporters => Core.Reporters.Reporters.Instance;
        internal static ILogger Logger { get; } = Dependencies.Instance.LoggerFactory.CreateLogger(typeof(Hole).FullName);

        private static readonly Lazy<IDictionary<HoleLocation, HoleInformation>> lazyHoleLookup = new Lazy<IDictionary<HoleLocation, HoleInformation>>(
            () =>
            {
                //return (IDictionary<HoleLocation, HoleInformation>)Type
                //                .GetType("Holedriven.Lookup")
                //                .GetField("Holes", BindingFlags.Public | BindingFlags.Static)
                //                .GetValue(null);
                var entry = Assembly.GetEntryAssembly();
                var @class = entry.GetType("Holedriven.Lookup");
                var property = @class.GetField("Holes", BindingFlags.Public | BindingFlags.Static);
                var lookup = property.GetValue(null);
                return (IDictionary<HoleLocation, HoleInformation>)lookup;
            });
        private static IDictionary<HoleLocation, HoleInformation> HoleLookup => lazyHoleLookup.Value;
        
        private static TInformation GetHoleInformation<TInformation>(string callerFilePath, int callerLineNumber, string callerMemberName)
            where TInformation : HoleInformation
        {
            if (HoleLookup == null)
                Logger.LogCritical("Lookup could not be found, please make sure that the Source Generator is installed correctly! (Instructions: https://holey.dev/docs/dotnet/codegen)");

            var location = new HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            return (TInformation)HoleLookup[location];
        }
        private static void ReportHoleEncountered(HoleType type, HoleInformation information)
        {
            Logger.LogDebug(
                HoleLogEvents.HoleEncountered,
                "Hole.{HoleType} encountered ({HoleInformation})",
                type,
                information);

            Reporters.InvokeHoleEncountered(information);
        }

        #region Fake
        public static TValue Fake<TValue>(
            string description,
            TValue value,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // get location
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            ReportFakeProvideHappened(information, value);
            return value;
        }

        public static Task<TValue> Fake<TValue>(
            string description,
            Task<TValue> asyncValue,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (asyncValue == null) throw new ArgumentNullException(nameof(asyncValue));

            // get information
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            return asyncValue.WithReporting(information, new Guid());
        }

        public static TValue Fake<TValue>(
            string description,
            Func<TValue> valueProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (valueProvider == null) throw new ArgumentNullException(nameof(valueProvider));

            // get information
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            var value = valueProvider.Invoke();
            ReportFakeProvideHappened(information, value);
            return value;
        }

        public static async Task<TValue> Fake<TValue>(
            string description,
            Func<Task<TValue>> asyncValueProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (asyncValueProvider == null) throw new ArgumentNullException(nameof(asyncValueProvider));

            // get information
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            return await asyncValueProvider.Invoke().WithReporting(information, new Guid());
        }

        public static TValue Fake<TValue>(
            string description,
            FakeProvider<TValue> fakeValueProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (fakeValueProvider == null) throw new ArgumentNullException(nameof(fakeValueProvider));

            // get information
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            var value = fakeValueProvider(new FakeExtensionInput(information));
            ReportFakeProvideHappened(information, value);
            return value;
        }

        public static Task<TValue> Fake<TValue>(
            string description,
            FakeProvider<Task<TValue>> fakeValueProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (fakeValueProvider == null) throw new ArgumentNullException(nameof(fakeValueProvider));

            // get information
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            return fakeValueProvider(new FakeExtensionInput(information)).WithReporting(information, new Guid());
        }

        public static void Fake(
            string description,
            Action effect,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (effect == null) throw new ArgumentNullException(nameof(effect));
            
            // get information
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            effect.Invoke();
            ReportFakeEffectHappened(information);
        }

        public static Task Fake(
            string description,
            Task asyncEffect,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (asyncEffect == null) throw new ArgumentNullException(nameof(asyncEffect));
            
            // get information
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            return asyncEffect.WithReporting(information, new Guid());
        }

        public static Task Fake(
            string description,
            Func<Task> asyncEffectProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (asyncEffectProvider == null) throw new ArgumentNullException(nameof(asyncEffectProvider));
            
            // get information
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            return asyncEffectProvider.Invoke().WithReporting(information, new Guid());
        }

        public static void Fake(
            string description,
            FakeVoidProvider fakeEffectProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (fakeEffectProvider == null) throw new ArgumentNullException(nameof(fakeEffectProvider));

            // get information
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            fakeEffectProvider(new FakeExtensionInput(information));
            ReportFakeEffectHappened(information);
        }

        public static Task Fake(
            string description,
            FakeProvider<Task> fakeAsyncEffectProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (fakeAsyncEffectProvider == null) throw new ArgumentNullException(nameof(fakeAsyncEffectProvider));

            // get information
            var information = GetHoleInformation<HoleInformationFake>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Fake, information);

            // execute hole
            return fakeAsyncEffectProvider(new FakeExtensionInput(information)).WithReporting(information, new Guid());
        }

        public delegate TValue FakeProvider<TValue>(IFakeExtension extension);
        public delegate void FakeVoidProvider(IFakeExtension extension);

        private class FakeExtensionInput : IFakeExtension
        {
            public HoleInformationFake Information { get; }
            public FakeExtensionInput(HoleInformationFake information) => Information = information;
        }
        
        private static void ReportFakeProvideHappened(HoleInformationFake information, object value)
        {
            Logger.LogInformation(
                HoleLogEvents.FakeProvideHappened,
                "Value faked: {FakedValue} ({HoleInformation})",
                value,
                information);

            Reporters.InvokeFakeProvideHappened(information, value);
        }
        private static void ReportFakeAsyncProvideStarted(HoleInformationFake information, Guid fakeId)
        {
            Logger.LogInformation(
                HoleLogEvents.FakeAsyncProvideStarted,
                "Faking of value started ({FakeId}, {HoleInformation})",
                fakeId,
                information);

            Reporters.InvokeFakeAsyncProvideStarted(information, fakeId);
        }
        private static void ReportFakeAsyncProvideCompleted(HoleInformationFake information, Guid fakeId, object value)
        {
            Logger.LogInformation(
                HoleLogEvents.FakeAsyncProvideStarted,
                "Faking of value completed: {Value} ({FakeId}, {HoleInformation})",
                value,
                fakeId,
                information);

            Reporters.InvokeFakeAsyncProvideCompleted(information, fakeId, value);
        }
        private static void ReportFakeAsyncProvideFaulted(HoleInformationFake information, Guid fakeId, Exception exception)
        {
            Logger.LogInformation(
                HoleLogEvents.FakeAsyncProvideStarted,
                "Faking of value faulted: {Exception} ({FakeId}, {HoleInformation})",
                exception,
                fakeId,
                information);

            Reporters.InvokeFakeAsyncProvideFaulted(information, fakeId, exception);
        }
        private static void ReportFakeAsyncProvideCanceled(HoleInformationFake information, Guid fakeId)
        {
            Logger.LogInformation(
                HoleLogEvents.FakeAsyncProvideStarted,
                "Faking of value was canceled ({FakeId}, {HoleInformation})",
                fakeId,
                information);

            Reporters.InvokeFakeAsyncProvideCanceled(information, fakeId);
        }
        private static void ReportFakeEffectHappened(HoleInformationFake information)
        {
            Logger.LogInformation(
                HoleLogEvents.FakeEffectHappened,
                "Effect faked {HoleInformation}",
                information);

            Reporters.InvokeFakeEffectHappened(information);
        }
        private static void ReportFakeAsyncEffectStarted(HoleInformationFake information, Guid fakeId)
        {
            Logger.LogInformation(
                HoleLogEvents.FakeAsyncEffectStarted,
                "Faking of effect started ({FakeId}, {HoleInformation})",
                fakeId,
                information);

            Reporters.InvokeFakeAsyncEffectStarted(information, fakeId);
        }
        private static void ReportFakeAsyncEffectCompleted(HoleInformationFake information, Guid fakeId)
        {
            Logger.LogInformation(
                HoleLogEvents.FakeAsyncEffectStarted,
                "Faking of effect completed ({FakeId}, {HoleInformation})",
                fakeId,
                information);

            Reporters.InvokeFakeAsyncEffectCompleted(information, fakeId);
        }
        private static void ReportFakeAsyncEffectFaulted(HoleInformationFake information, Guid fakeId, Exception exception)
        {
            Logger.LogInformation(
                HoleLogEvents.FakeAsyncEffectStarted,
                "Faking of effect faulted: {Exception} ({FakeId}, {HoleInformation})",
                exception,
                fakeId,
                information);

            Reporters.InvokeFakeAsyncEffectFaulted(information, fakeId, exception);
        }
        private static void ReportFakeAsyncEffectCanceled(HoleInformationFake information, Guid fakeId)
        {
            Logger.LogInformation(
                HoleLogEvents.FakeAsyncEffectStarted,
                "Faking of effect was canceled ({FakeId}, {HoleInformation})",
                fakeId,
                information);

            Reporters.InvokeFakeAsyncEffectCanceled(information, fakeId);
        }

        private static Task<T> WithReporting<T>(this Task<T> task, HoleInformationFake information, Guid fakeId)
        {
            ReportFakeAsyncProvideStarted(information, fakeId);
            return task.ContinueWith(originalTask =>
            {
                switch (originalTask.Status)
                {
                    case TaskStatus.RanToCompletion: ReportFakeAsyncProvideCompleted(information, fakeId, originalTask.Result); break;
                    case TaskStatus.Faulted: ReportFakeAsyncProvideFaulted(information, fakeId, originalTask.Exception); break;
                    case TaskStatus.Canceled: ReportFakeAsyncProvideCanceled(information, fakeId); break;
                    default: throw new NotSupportedException($"{originalTask.Status} is not supported");
                }
                return originalTask.Result;
            });
        }
        private static Task WithReporting(this Task task, HoleInformationFake holeId, Guid fakeId)
        {
            ReportFakeAsyncEffectStarted(holeId, fakeId);
            return task.ContinueWith(originalTask =>
            {
                switch (originalTask.Status)
                {
                    case TaskStatus.RanToCompletion: ReportFakeAsyncEffectCompleted(holeId, fakeId); break;
                    case TaskStatus.Faulted: ReportFakeAsyncEffectFaulted(holeId, fakeId, originalTask.Exception); break;
                    case TaskStatus.Canceled: ReportFakeAsyncEffectCanceled(holeId, fakeId); break;
                    default: throw new NotSupportedException($"{originalTask.Status} is not supported");
                }
            });
        }
        #endregion
        #region Idea
        public static void Idea(
            string description,
            HoleScope scope = HoleScope.Nearest,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var information = GetHoleInformation<HoleInformationIdea>(callerFilePath, callerLineNumber, callerMemberName);
            Reporters.InvokeHoleEncountered(information);
        }
        #endregion
        #region Refactor
        public static void Refactor(
            string description,
            Action source,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (source == null) throw new ArgumentNullException(nameof(source));

            // get information
            var information = GetHoleInformation<HoleInformationRefactor>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Refactor, information);

            // execute hole
            source.Invoke();
        }

        public static TValue Refactor<TValue>(
            string description,
            Func<TValue> source,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // validate mandatory parameters
            if (source == null) throw new ArgumentNullException(nameof(source));

            // get information
            var information = GetHoleInformation<HoleInformationRefactor>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Refactor, information);

            // execute hole
            return source.Invoke();
        }

        [Idea("enable marking of Blocks/Scopes, e.g. NextLine, following if/switch/loop, also make sure to check the correct usage of those scopes with an analyzer")]
        public static void Refactor<T>(
            string description,
            HoleScope scope,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // get information
            var information = GetHoleInformation<HoleInformationRefactor>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Refactor, information);
        }
        #endregion
        #region NotImplemented
        [DebuggerHidden]
        public static NotImplementedException NotImplemented(
            string description,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            // get information
            var information = GetHoleInformation<HoleInformationRefactor>(callerFilePath, callerLineNumber, callerMemberName);

            // report hole encountered
            ReportHoleEncountered(HoleType.Refactor, information);

            // execute hole
            return new HoleNotFilledException(description);
        }
        #endregion

        #region Attributes
        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        public class IdeaAttribute : Attribute
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This description is only used in the Source Code Analyzer")]
            public IdeaAttribute(string Description)
            {

            }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        public class RefactorAttribute : Attribute
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This description is only used in the Source Code Analyzer")]
            public RefactorAttribute(string Description)
            {

            }
        }
        #endregion
    }
}
