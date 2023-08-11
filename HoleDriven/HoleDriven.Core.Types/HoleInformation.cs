using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HoleDriven.Core.Types
{
    public abstract class HoleInformation
    {
        protected HoleInformation(HoleType type, HoleLocation location, string description)
        {
            Type = type;
            Location = location;
            Description = description;
        }

        public HoleType Type { get; }
        public HoleLocation Location { get; }
        public string Description { get; }
    }

    public class HoleInformationFake : HoleInformation
    {
        public HoleInformationFake(HoleLocation location, string description) : base(HoleType.Fake, location, description)
        {
        }
    }
    public class HoleInformationIdea : HoleInformation
    {
        public HoleInformationIdea(HoleLocation location, string description, HoleScope scope) : base(HoleType.Idea, location, description)
        {
            Scope = scope;
        }

        public HoleScope Scope { get; }
    }
    public class HoleInformationRefactor : HoleInformation
    {
        public HoleInformationRefactor(HoleLocation location, string description, string source) : base(HoleType.Refactor, location, description)
        {
            Source = source;
        }

        public string Source { get; }
    }
    public class HoleInformationNotImplemented : HoleInformation
    {
        public HoleInformationNotImplemented(HoleLocation location, string description) : base(HoleType.NotImplemented, location, description)
        {
        }
    }
}
