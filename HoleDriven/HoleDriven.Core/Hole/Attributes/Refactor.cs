using System;

namespace HoleDriven
{
    public static partial class Hole
    {
        public class RefactorAttribute : Attribute
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This description is only used in the Source Code Analyzer")]
            public RefactorAttribute(string Description)
            {

            }
        }
    }
}
