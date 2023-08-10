using HoleDriven.Core.Types;

namespace HoleDriven.Core.Reporters
{
    public class HoleEncountered
    {
        public delegate void Delegate(Params hole);
        public class Params : BaseReporterParams
        {
            public Params(HoleType type, HoleLocation location, string description) : base(location, description)
            {
                Type = type;
            }

            public HoleType Type { get; }
        }
    }
}
