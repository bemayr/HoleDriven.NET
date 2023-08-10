using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HoleDriven.Core.Types
{
    public abstract class HoleInformation
    {
        public string Description { get; }
        public HoleLocation Location { get; }
    }
}
