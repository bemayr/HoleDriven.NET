using HoleDriven.Core.Types;

namespace HoleDriven.Core.Reporters
{
    [Hole.Idea("Maybe AsyncFakeHappened would be a nice addition")]
    public class FakeHappened
    {
        public delegate void Delegate(Params hole);
        public class Params : BaseReporterParams
        {
            public Params(object value, HoleLocation location, string description) : base(location, description)
            {
                Value = value;
            }

            public object Value { get; }
        }
    }
}
