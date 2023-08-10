using HoleDriven.Core.Types;

namespace HoleDriven.Core.Reporters
{
    public abstract class BaseReporterParams
    {
        protected BaseReporterParams(HoleLocation location, string description)
        {
            Location = location;
            Description = description;
        }

        public HoleLocation Location { get; }
        public string Description { get; }
    }
}
