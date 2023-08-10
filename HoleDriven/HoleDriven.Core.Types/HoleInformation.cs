using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HoleDriven.Core.Types
{
    public abstract class HoleInformation
    {
        protected HoleInformation(HoleLocation location, string description)
        {
            Description = description;
            Location = location;
        }

        public HoleLocation Location { get; }
        public string Description { get; }
    }

    public class HoleInformationNotImplemented : HoleInformation
    {
        public HoleInformationNotImplemented(HoleLocation location, string description) : base(location, description)
        {
        }
    }
    public class HoleInformationFake : HoleInformation
    {
        public HoleInformationFake(HoleLocation location, string description) : base(location, description)
        {
        }
    }
}
