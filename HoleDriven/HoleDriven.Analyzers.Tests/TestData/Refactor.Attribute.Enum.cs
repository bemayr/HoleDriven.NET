﻿using HoleDriven;

namespace HoleDriven.Analyzers.Tests.TestData
{
    [Hole.Refactor("/*description*/")]
    public enum /*{|ExpectedDiagnosticLocation:*/Refactor_Attribute_Enum/*|}*/
    {
    }
}
