﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Holedriven
{
    public static partial class Hole
    {
        public interface ISetAsyncResult
        {
            Task Task { get; }
        }

        public class SetAsyncResultCompleted : ISetAsyncResult
        {
            public Task Task => Task.CompletedTask;
        }

        public delegate ISetAsyncResult SetAsyncResultProvider(string a);

        public static Task EffectAsync(
            string description,
            SetAsyncResultProvider resultProvider = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            resultProvider = resultProvider ?? (_ => new SetAsyncResultCompleted());

            ReportHole(nameof(EffectAsync), $"{description} ({new { callerFilePath, callerLineNumber, callerMemberName }})");
            return resultProvider("what the fuck does this string do").Task; // TODO: replace with a hole and check whether this is needed
        }
    }
}