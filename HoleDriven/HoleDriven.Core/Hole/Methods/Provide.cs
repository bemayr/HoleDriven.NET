﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static HoleDriven.Hole;

namespace HoleDriven
{
    public static partial class Hole
    {
        public delegate TValue ProvideValueProvider<TValue>(IProvideInput hole);

        public interface IProvideInput
        {
            string Description { get; }
        }

        internal class ProvideInput : IProvideInput
        {
            public string Description { get; }
            public ProvideInput(string description) => Description = description;
        }

        public static TValue Provide<TValue>(
            string description,
            TValue value,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            Report.HoleEncountered(description, location);
            Report.ProvideHappened(description, value, location);
            return value;
        }

        public static TValue Provide<TValue>(
            string description,
            ProvideValueProvider<TValue> valueProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            var value = valueProvider(new ProvideInput(description));

            Report.HoleEncountered(description, location);
            Report.ProvideHappened(description, value, location);
            return value;
        }
    }
}
