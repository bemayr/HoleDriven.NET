using HoleDriven.Core.Types;
using System;

namespace HoleDriven.Core.Reporters
{
    public class Reporters : IReporters, IReportable
    {
        private static readonly Lazy<Reporters> lazy = new Lazy<Reporters>(() => new Reporters());
        public static Reporters Instance => lazy.Value;

        public event HoleEncountered.Delegate HoleEncountered;
        public event FakeHappened.Delegate FakeHappened;

        private Reporters() { }

        public void InvokeHoleEncountered(HoleType type, HoleLocation location, string description) =>
            HoleEncountered?.Invoke(new HoleEncountered.Params(type, location, description));
        public void InvokeFakeHappened(object value, HoleLocation location, string description) =>
            FakeHappened?.Invoke(new FakeHappened.Params(value, location, description));
    }
}
